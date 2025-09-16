// 簽核管理 JavaScript

// 全域變數
let currentHistoryPage = 1;
let currentHistoryPageSize = 10;

// 頁面載入完成後初始化
$(document).ready(function() {
    initializeApprovalPage();
    bindEvents();
});

// 初始化頁面
function initializeApprovalPage() {
    loadPendingApprovals();
    loadInProgressApprovals();
    loadApprovalHistory(1, currentHistoryPageSize);
}

// 綁定事件
function bindEvents() {
    // 歷史頁面大小改變事件
    $('#historyPageSize').change(function() {
        currentHistoryPageSize = parseInt($(this).val());
        currentHistoryPage = 1;
        loadApprovalHistory(currentHistoryPage, currentHistoryPageSize);
    });

    // 簽核動作類型改變事件
    $('#actionType').change(function() {
        const actionType = $(this).val();
        const nextApproverGroup = $('#nextApproverGroup');
        
        if (actionType === 'forward') {
            nextApproverGroup.show();
            $('#nextApprover').prop('required', true);
        } else {
            nextApproverGroup.hide();
            $('#nextApprover').prop('required', false);
        }
    });
}

// 載入簽核待辦列表
function loadPendingApprovals() {
    showLoading('#pendingApprovalsTable');
    
    $.get('/Approval/GetPendingApprovals')
        .done(function(response) {
            if (response.success) {
                renderPendingApprovalsTable(response.data);
                updatePendingBadge(response.data.length);
                updateStatistics();
            } else {
                showError('#pendingApprovalsTable', response.message);
            }
        })
        .fail(function() {
            showError('#pendingApprovalsTable', '載入簽核待辦列表失敗');
            updatePendingBadge(0);
        });
}

// 載入即時流程列表
function loadInProgressApprovals() {
    showLoading('#inProgressApprovalsTable');
    
    $.get('/Approval/GetInProgressApprovals')
        .done(function(response) {
            if (response.success) {
                renderInProgressApprovalsTable(response.data);
                updateProgressBadge(response.data.length);
                updateStatistics();
            } else {
                showError('#inProgressApprovalsTable', response.message);
            }
        })
        .fail(function() {
            showError('#inProgressApprovalsTable', '載入即時流程列表失敗');
            updateProgressBadge(0);
        });
}

// 載入簽核歷史列表
function loadApprovalHistory(page = 1, pageSize = 10) {
    showLoading('#approvalHistoryTable');
    
    $.get('/Approval/GetApprovalHistory', { page: page, pageSize: pageSize })
        .done(function(response) {
            if (response.success) {
                renderApprovalHistoryTable(response.data);
                renderPagination(response.currentPage, response.pageSize, response.totalCount);
                updateHistoryBadge(response.totalCount);
                currentHistoryPage = response.currentPage;
                updateStatistics();
            } else {
                showError('#approvalHistoryTable', response.message);
            }
        })
        .fail(function() {
            showError('#approvalHistoryTable', '載入簽核歷史列表失敗');
            updateHistoryBadge(0);
        });
}

// 渲染簽核待辦表格
function renderPendingApprovalsTable(data) {
    if (!data || data.length === 0) {
        $('#pendingApprovalsTable').html('<div class="no-data">目前沒有待簽核項目</div>');
        updatePendingBadge(0);
        return;
    }

    let html = `
        <div class="table-responsive">
            <table class="table table-hover">
                <thead class="table-light">
                    <tr>
                        <th>標題</th>
                        <th>申請人</th>
                        <th>類型</th>
                        <th>金額</th>
                        <th>優先級</th>
                        <th>到期日</th>
                        <th>操作</th>
                    </tr>
                </thead>
                <tbody>
    `;

    data.forEach(item => {
        const dueDate = item.dueDate ? new Date(item.dueDate).toLocaleDateString() : '無';
        const isOverdue = item.daysOverdue > 0;
        const overdueClass = isOverdue ? 'table-danger' : '';
        const priorityBadge = getPriorityBadge(item.priority);
        const amountDisplay = item.amount ? `$${item.amount.toLocaleString()}` : '-';

        html += `
            <tr class="${overdueClass}">
                <td>
                    <div class="fw-bold">${escapeHtml(item.title)}</div>
                    ${isOverdue ? '<small class="text-danger">已逾期 ' + item.daysOverdue + ' 天</small>' : ''}
                </td>
                <td>${escapeHtml(item.requestUser)}</td>
                <td><span class="badge bg-secondary">${escapeHtml(item.approvalType)}</span></td>
                <td>${amountDisplay}</td>
                <td>${priorityBadge}</td>
                <td>${dueDate}</td>
                <td>
                    <button class="btn btn-sm btn-outline-primary me-1" onclick="viewApprovalDetail(${item.id})" title="詳情">
                        <i class="fas fa-eye"></i>
                    </button>
                    <button class="btn btn-sm btn-outline-info me-1" onclick="viewApprovalHistory(${item.id})" title="歷史記錄">
                        <i class="fas fa-history"></i>
                    </button>
                    <button class="btn btn-sm btn-success" onclick="showApprovalActionModal(${item.id})" title="處理">
                        <i class="fas fa-check"></i>
                    </button>
                </td>
            </tr>
        `;
    });

    html += '</tbody></table></div>';
    $('#pendingApprovalsTable').html(html);
}

// 渲染即時流程表格
function renderInProgressApprovalsTable(data) {
    if (!data || data.length === 0) {
        $('#inProgressApprovalsTable').html('<div class="no-data">目前沒有進行中的流程</div>');
        updateProgressBadge(0);
        return;
    }

    let html = `
        <div class="table-responsive">
            <table class="table table-hover">
                <thead class="table-light">
                    <tr>
                        <th>標題</th>
                        <th>申請人</th>
                        <th>目前簽核人</th>
                        <th>類型</th>
                        <th>狀態</th>
                        <th>最後動作</th>
                        <th>操作</th>
                    </tr>
                </thead>
                <tbody>
    `;

    data.forEach(item => {
        const statusBadge = getStatusBadge(item.status);
        const priorityBadge = getPriorityBadge(item.priority);
        const lastActionDate = new Date(item.lastActionDate).toLocaleDateString();

        html += `
            <tr>
                <td>
                    <div class="fw-bold">${escapeHtml(item.title)}</div>
                    ${priorityBadge}
                </td>
                <td>${escapeHtml(item.requestUser)}</td>
                <td>${escapeHtml(item.currentApprover)}</td>
                <td><span class="badge bg-secondary">${escapeHtml(item.approvalType)}</span></td>
                <td>${statusBadge}</td>
                <td>
                    <div>${escapeHtml(item.lastAction)}</div>
                    <small class="text-muted">${lastActionDate}</small>
                </td>
                <td>
                    <button class="btn btn-sm btn-outline-primary me-1" onclick="viewApprovalDetail(${item.id})" title="詳情">
                        <i class="fas fa-eye"></i>
                    </button>
                    <button class="btn btn-sm btn-outline-info" onclick="viewApprovalHistory(${item.id})" title="歷史記錄">
                        <i class="fas fa-history"></i>
                    </button>
                </td>
            </tr>
        `;
    });

    html += '</tbody></table></div>';
    $('#inProgressApprovalsTable').html(html);
}

// 渲染簽核歷史表格
function renderApprovalHistoryTable(data) {
    if (!data || data.length === 0) {
        $('#approvalHistoryTable').html('<div class="no-data">目前沒有歷史記錄</div>');
        updateHistoryBadge(0);
        return;
    }

    let html = `
        <div class="table-responsive">
            <table class="table table-hover">
                <thead class="table-light">
                    <tr>
                        <th>標題</th>
                        <th>申請人</th>
                        <th>類型</th>
                        <th>最終狀態</th>
                        <th>完成日期</th>
                        <th>最終簽核人</th>
                        <th>操作</th>
                    </tr>
                </thead>
                <tbody>
    `;

    data.forEach(item => {
        const statusBadge = getStatusBadge(item.status);
        const completedDate = item.completedDate ? new Date(item.completedDate).toLocaleDateString() : '-';

        html += `
            <tr>
                <td class="fw-bold">${escapeHtml(item.title)}</td>
                <td>${escapeHtml(item.requestUser)}</td>
                <td><span class="badge bg-secondary">${escapeHtml(item.approvalType)}</span></td>
                <td>${statusBadge}</td>
                <td>${completedDate}</td>
                <td>${escapeHtml(item.finalApprover)}</td>
                <td>
                    <button class="btn btn-sm btn-outline-primary me-1" onclick="viewApprovalDetail(${item.id})" title="詳情">
                        <i class="fas fa-eye"></i>
                    </button>
                    <button class="btn btn-sm btn-outline-info" onclick="viewApprovalHistory(${item.id})" title="歷史記錄">
                        <i class="fas fa-history"></i>
                    </button>
                </td>
            </tr>
        `;
    });

    html += '</tbody></table></div>';
    $('#approvalHistoryTable').html(html);
}

// 渲染分頁
function renderPagination(currentPage, pageSize, totalCount) {
    const totalPages = Math.ceil(totalCount / pageSize);
    
    if (totalPages <= 1) {
        $('#historyPagination').html('');
        return;
    }

    let html = '<nav><ul class="pagination">';
    
    // 上一頁
    if (currentPage > 1) {
        html += `<li class="page-item">
            <a class="page-link" href="#" onclick="loadApprovalHistory(${currentPage - 1}, ${pageSize})">上一頁</a>
        </li>`;
    }

    // 頁碼
    const startPage = Math.max(1, currentPage - 2);
    const endPage = Math.min(totalPages, currentPage + 2);

    for (let i = startPage; i <= endPage; i++) {
        const activeClass = i === currentPage ? 'active' : '';
        html += `<li class="page-item ${activeClass}">
            <a class="page-link" href="#" onclick="loadApprovalHistory(${i}, ${pageSize})">${i}</a>
        </li>`;
    }

    // 下一頁
    if (currentPage < totalPages) {
        html += `<li class="page-item">
            <a class="page-link" href="#" onclick="loadApprovalHistory(${currentPage + 1}, ${pageSize})">下一頁</a>
        </li>`;
    }

    html += '</ul></nav>';
    $('#historyPagination').html(html);
}

// 顯示簽核詳情
function viewApprovalDetail(approvalId) {
    $.get('/Approval/GetApprovalDetail', { id: approvalId })
        .done(function(response) {
            if (response.success) {
                renderApprovalDetail(response.data);
                $('#approvalDetailModal').modal('show');
            } else {
                showAlert('錯誤', response.message, 'error');
            }
        })
        .fail(function() {
            showAlert('錯誤', '載入簽核詳情失敗', 'error');
        });
}

// 顯示簽核歷史記錄
function viewApprovalHistory(approvalId) {
    $.get('/Approval/GetApprovalDetail', { id: approvalId })
        .done(function(response) {
            if (response.success) {
                renderApprovalHistoryModal(response.data);
                $('#approvalHistoryModal').modal('show');
            } else {
                showAlert('錯誤', response.message, 'error');
            }
        })
        .fail(function() {
            showAlert('錯誤', '載入簽核歷史失敗', 'error');
        });
}

// 渲染簽核詳情
function renderApprovalDetail(data) {
    const statusBadge = getStatusBadge(data.status);
    const priorityBadge = getPriorityBadge(data.priority);
    const createdDate = new Date(data.createdDate).toLocaleString();
    const dueDate = data.dueDate ? new Date(data.dueDate).toLocaleString() : '無';
    const completedDate = data.completedDate ? new Date(data.completedDate).toLocaleString() : '未完成';
    const amountDisplay = data.amount ? `$${data.amount.toLocaleString()}` : '無';

    let html = `
        <div class="approval-detail">
            <div class="row mb-3">
                <div class="col-md-6">
                    <strong>標題：</strong>${escapeHtml(data.title)}
                </div>
                <div class="col-md-6">
                    <strong>狀態：</strong>${statusBadge}
                </div>
            </div>
            <div class="row mb-3">
                <div class="col-md-6">
                    <strong>申請人：</strong>${escapeHtml(data.requestUser)}
                </div>
                <div class="col-md-6">
                    <strong>目前簽核人：</strong>${escapeHtml(data.currentApprover)}
                </div>
            </div>
            <div class="row mb-3">
                <div class="col-md-6">
                    <strong>類型：</strong><span class="badge bg-secondary">${escapeHtml(data.approvalType)}</span>
                </div>
                <div class="col-md-6">
                    <strong>優先級：</strong>${priorityBadge}
                </div>
            </div>
            <div class="row mb-3">
                <div class="col-md-6">
                    <strong>金額：</strong>${amountDisplay}
                </div>
                <div class="col-md-6">
                    <strong>申請日期：</strong>${createdDate}
                </div>
            </div>
            <div class="row mb-3">
                <div class="col-md-6">
                    <strong>到期日期：</strong>${dueDate}
                </div>
                <div class="col-md-6">
                    <strong>完成日期：</strong>${completedDate}
                </div>
            </div>
    `;

    if (data.description) {
        html += `
            <div class="mb-3">
                <strong>描述：</strong>
                <div class="mt-1">${escapeHtml(data.description)}</div>
            </div>
        `;
    }

    if (data.remarks) {
        html += `
            <div class="mb-3">
                <strong>備註：</strong>
                <div class="mt-1">${escapeHtml(data.remarks)}</div>
            </div>
        `;
    }

    // 簽核歷史
    if (data.histories && data.histories.length > 0) {
        html += `
            <div class="mb-3">
                <strong>簽核歷史：</strong>
                <div class="mt-2">
        `;

        data.histories.forEach(history => {
            const actionDate = new Date(history.actionDate).toLocaleString();
            const actionBadge = getActionBadge(history.action);
            
            html += `
                <div class="history-item border-start ps-3 mb-2">
                    <div class="d-flex justify-content-between align-items-start">
                        <div>
                            <strong>${escapeHtml(history.approverName)}</strong>
                            ${actionBadge}
                        </div>
                        <small class="text-muted">${actionDate}</small>
                    </div>
                    ${history.comments ? `<div class="mt-1 text-muted">${escapeHtml(history.comments)}</div>` : ''}
                </div>
            `;
        });

        html += '</div></div>';
    }

    html += '</div>';
    $('#approvalDetailContent').html(html);
}

// 渲染簽核歷史模態框
function renderApprovalHistoryModal(data) {
    let html = `
        <div class="approval-history-summary mb-4">
            <div class="row">
                <div class="col-md-8">
                    <h6 class="fw-bold mb-2">${escapeHtml(data.title)}</h6>
                    <div class="text-muted">
                        <span class="me-3"><i class="fas fa-user"></i> 申請人：${escapeHtml(data.requestUser)}</span>
                        <span class="me-3"><i class="fas fa-tag"></i> 類型：${escapeHtml(data.approvalType)}</span>
                        ${data.amount ? `<span class="me-3"><i class="fas fa-dollar-sign"></i> 金額：$${data.amount.toLocaleString()}</span>` : ''}
                    </div>
                </div>
                <div class="col-md-4 text-end">
                    ${getStatusBadge(data.status)}
                    <div class="text-muted small mt-1">
                        ${new Date(data.createdDate).toLocaleDateString()}
                    </div>
                </div>
            </div>
        </div>
    `;

    if (data.histories && data.histories.length > 0) {
        html += `
            <div class="approval-timeline">
                <h6 class="mb-3">
                    <i class="fas fa-route me-2"></i>
                    處理流程
                </h6>
        `;

        // 按時間順序排列歷史記錄
        const sortedHistories = [...data.histories].sort((a, b) => new Date(a.actionDate) - new Date(b.actionDate));
        
        sortedHistories.forEach((history, index) => {
            const actionDate = new Date(history.actionDate).toLocaleString();
            const actionBadge = getActionBadge(history.action);
            const isLast = index === sortedHistories.length - 1;
            
            html += `
                <div class="timeline-item ${isLast ? 'timeline-item-last' : ''}">
                    <div class="timeline-marker">
                        <i class="fas fa-circle"></i>
                    </div>
                    <div class="timeline-content">
                        <div class="d-flex justify-content-between align-items-start mb-1">
                            <div>
                                <strong>${escapeHtml(history.approverName)}</strong>
                                ${actionBadge}
                            </div>
                            <small class="text-muted">${actionDate}</small>
                        </div>
                        ${history.comments ? `
                            <div class="timeline-comment mt-2">
                                <i class="fas fa-quote-left text-muted me-1"></i>
                                <span class="text-muted">${escapeHtml(history.comments)}</span>
                            </div>
                        ` : ''}
                    </div>
                </div>
            `;
        });

        html += '</div>';
    } else {
        html += `
            <div class="no-history text-center py-4">
                <i class="fas fa-history text-muted mb-2" style="font-size: 2rem;"></i>
                <div class="text-muted">尚無處理記錄</div>
            </div>
        `;
    }

    // 如果有描述或備註，顯示額外資訊
    if (data.description || data.remarks) {
        html += '<hr class="my-4">';
        
        if (data.description) {
            html += `
                <div class="mb-3">
                    <strong><i class="fas fa-info-circle me-1"></i>申請描述：</strong>
                    <div class="mt-1 p-2 bg-light rounded">${escapeHtml(data.description)}</div>
                </div>
            `;
        }
        
        if (data.remarks) {
            html += `
                <div class="mb-3">
                    <strong><i class="fas fa-sticky-note me-1"></i>備註：</strong>
                    <div class="mt-1 p-2 bg-light rounded">${escapeHtml(data.remarks)}</div>
                </div>
            `;
        }
    }

    $('#approvalHistoryContent').html(html);
}

// 顯示簽核動作模態框
function showApprovalActionModal(approvalId) {
    $('#actionApprovalId').val(approvalId);
    $('#approvalActionForm')[0].reset();
    $('#nextApproverGroup').hide();
    $('#nextApprover').prop('required', false);
    $('#approvalActionModal').modal('show');
}

// 提交簽核動作
function submitApprovalAction() {
    const form = $('#approvalActionForm')[0];
    if (!form.checkValidity()) {
        form.reportValidity();
        return;
    }

    const actionData = {
        approvalId: parseInt($('#actionApprovalId').val()),
        action: $('#actionType').val(),
        comments: $('#actionComments').val(),
        nextApprover: $('#nextApprover').val()
    };

    $.ajax({
        url: '/Approval/ProcessApproval',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(actionData)
    })
    .done(function(response) {
        if (response.success) {
            $('#approvalActionModal').modal('hide');
            showAlert('成功', response.message, 'success');
            // 重新載入數據
            initializeApprovalPage();
        } else {
            showAlert('錯誤', response.message, 'error');
        }
    })
    .fail(function() {
        showAlert('錯誤', '提交簽核動作失敗', 'error');
    });
}

// 重新整理函數
function refreshPendingApprovals() {
    loadPendingApprovals();
}

function refreshInProgressApprovals() {
    loadInProgressApprovals();
}

function refreshApprovalHistory() {
    loadApprovalHistory(currentHistoryPage, currentHistoryPageSize);
}

// 更新統計數字
function updateStatistics() {
    // 這裡可以從已載入的數據計算統計，或另外調用API
    // 為簡化，這裡只是示例
    setTimeout(() => {
        const pendingCount = $('#pendingApprovalsTable tbody tr').length || 0;
        const progressCount = $('#inProgressApprovalsTable tbody tr').length || 0;
        const completedCount = $('#approvalHistoryTable tbody tr').length || 0;
        
        $('#pendingCount').text(pendingCount);
        $('#progressCount').text(progressCount);
        $('#completedCount').text(completedCount);
    }, 100);
}

// 更新待簽核 badge
function updatePendingBadge(count) {
    const badge = $('#pendingBadge');
    
    // 添加動畫效果
    badge.addClass('animate-update');
    setTimeout(() => badge.removeClass('animate-update'), 300);
    
    badge.text(count);
    
    // 根據數量改變 badge 顏色
    badge.removeClass('bg-warning bg-danger bg-success');
    if (count === 0) {
        badge.addClass('bg-success');
    } else if (count <= 5) {
        badge.addClass('bg-warning');
    } else {
        badge.addClass('bg-danger');
    }
}

// 更新進行中 badge
function updateProgressBadge(count) {
    const badge = $('#progressBadge');
    
    // 添加動畫效果
    badge.addClass('animate-update');
    setTimeout(() => badge.removeClass('animate-update'), 300);
    
    badge.text(count);
    
    // 根據數量改變 badge 顏色
    badge.removeClass('bg-info bg-primary bg-secondary');
    if (count === 0) {
        badge.addClass('bg-secondary');
    } else if (count <= 3) {
        badge.addClass('bg-info');
    } else {
        badge.addClass('bg-primary');
    }
}

// 更新歷史記錄 badge
function updateHistoryBadge(count) {
    const badge = $('#historyBadge');
    
    // 添加動畫效果
    badge.addClass('animate-update');
    setTimeout(() => badge.removeClass('animate-update'), 300);
    
    badge.text(count);
    
    // 歷史記錄保持灰色
    badge.removeClass('bg-secondary bg-dark');
    badge.addClass(count > 0 ? 'bg-secondary' : 'bg-dark');
}

// 輔助函數
function getStatusBadge(status) {
    const statusMap = {
        0: '<span class="badge bg-warning">待簽核</span>',
        1: '<span class="badge bg-info">進行中</span>',
        2: '<span class="badge bg-success">已同意</span>',
        3: '<span class="badge bg-danger">已拒絕</span>',
        4: '<span class="badge bg-secondary">已撤回</span>',
        5: '<span class="badge bg-dark">已過期</span>'
    };
    return statusMap[status] || '<span class="badge bg-secondary">未知</span>';
}

function getPriorityBadge(priority) {
    const priorityMap = {
        'High': '<span class="badge bg-danger">高</span>',
        'Normal': '<span class="badge bg-primary">普通</span>',
        'Low': '<span class="badge bg-secondary">低</span>'
    };
    return priorityMap[priority] || '<span class="badge bg-secondary">普通</span>';
}

function getActionBadge(action) {
    const actionMap = {
        'Approve': '<span class="badge bg-success">同意</span>',
        'Reject': '<span class="badge bg-danger">拒絕</span>',
        'Forward': '<span class="badge bg-info">轉送</span>',
        'Submit': '<span class="badge bg-primary">提交</span>'
    };
    return actionMap[action] || `<span class="badge bg-secondary">${action}</span>`;
}

function escapeHtml(text) {
    if (!text) return '';
    const div = document.createElement('div');
    div.textContent = text;
    return div.innerHTML;
}

function showLoading(selector) {
    $(selector).html('<div class="loading-spinner"><i class="fas fa-spinner fa-spin"></i> 載入中...</div>');
}

function showError(selector, message) {
    $(selector).html(`<div class="error-message text-danger"><i class="fas fa-exclamation-triangle"></i> ${message}</div>`);
}

function showAlert(title, message, type) {
    // 簡單的提示，可以替換為更好的提示組件
    const alertType = type === 'success' ? 'alert-success' : 'alert-danger';
    const alertHtml = `
        <div class="alert ${alertType} alert-dismissible fade show" role="alert">
            <strong>${title}：</strong>${message}
            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
        </div>
    `;
    
    // 在頁面頂部顯示提示
    if ($('.alert-container').length === 0) {
        $('.approval-container').prepend('<div class="alert-container"></div>');
    }
    $('.alert-container').html(alertHtml);
    
    // 3秒後自動隱藏
    setTimeout(() => {
        $('.alert').alert('close');
    }, 3000);
}