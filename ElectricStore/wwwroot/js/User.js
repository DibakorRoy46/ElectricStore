var dataTable;

$(document).ready(function () {
    loadDataTable();
});



function loadDataTable() {
    dataTable = $("#tblData").DataTable({
        "ajax": {
            "url": "/Admin/User/GetAll/"
        },
        "columns": [
            { "data": "name", "width": "12%" },
            { "data": "email", "width": "12%" },          
            { "data": "role", "width": "10%" },         
            {
                "data": {
                    id: "id", lockoutEnd: "lockoutEnd"
                },
                "render": function (data) {
                    var today = new Date().getTime();
                    var lockOut = new Date(data.lockoutEnd).getTime();
                    if (lockOut > today) {
                        return `<div class="text-center">     
                                    <a class="btn btn-danger"  onclick=LockUnlocked('${data.id}')><i class="fas fa-lock-open"></i> UnLock</a>
                            </div>`;
                    }
                    else {
                        return `<div class="text-center">     
                                    <a class="btn btn-success" onclick=LockUnlocked('${data.id}')><i class="fas fa-lock"></i> Lock</a>
                            </div>`;
                    }
                },"width":"14%"
            },
        ]


    });
}




function LockUnlocked(id) {
    $.ajax({
        type: "POST",
        url: "/Admin/User/LockUnlock",
        data: JSON.stringify(id),
        contentType: "application/json",
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
                dataTable.ajax.reload();
            }
            else {
                toastr.error(data.message);
            }
        }
    });
}


