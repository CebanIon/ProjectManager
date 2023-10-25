function goToProjectFromDashboard(projectId) {
    goToTaskManagerIndex();
    goToProject(projectId);
}

function goToTaskFromDashboard(taskId) {
    goToTaskManagerIndex();
    goToTaskDetails(taskId);
}

function startLoading() {
    $("#loader").show();
}

function endLoading() {
    $("#loader").hide();
}

function userpageLoad() {
    let table = $('#userTable').DataTable({
        processing: true,
        serverSide: true,
        "scrollX": true,
        "ajax": {
            url: "User/GetAllUsers",
            type: "POST"
        },
        "columns": [
            { "data": "id", title: "Id", name: "id", visible: false },
            { "data": "userName", title: "Username", name: "userName" },
            { "data": "firstName", title: "First Name", name: "firstName" },
            { "data": "lastName", title: "Last Name", name: "lastName" },
            { "data": "email", title: "Email", name: "email" },
            { "data": "isEnabled", title: "Enabled", name: "isEnabled" },
            { "data": "role", title: "Role", name: "role" }
        ]
    });
    let contextmenu = $('#userTable').contextMenu({
        selector: 'tr',
        trigger: 'right',
        callback: function (key, options) {
            console.log(table);
            console.log(table.rows());
            let row = table.row(options.$trigger);
            console.log(row.data());
            switch (key) {
                case 'details':
                    gotToUserDetailsPage(row.data()["id"]);
                    break;
                case 'edit':
                    gotToUserEditPage(row.data()["id"]);
                    break;
                default:
                    break
            }
        },
        items: {
            "edit": { name: "Edit" },
            "details": { name: "Details" }
        }
    })
};

function usersOfProjectLoad(projectId) {
    let table = $('#projectUserTable').DataTable({
        processing: true,
        serverSide: true,
        "ajax": {
            url: "TaskManager/GetUsersOfProject",
            type: "POST",
            xhrFields: {
                withCredentials: true
            },
            data: {
                projectId: projectId
            },
        },
        "columns": [
            { "data": "id", title: "Id", name: "id", visible: false },
            { "data": "userName", title: "UserName", name: "userName" },
            { "data": "firstName", title: "FirstName", name: "firstName" },
            { "data": "lastName", title: "LastName", name: "lastName" },
            { "data": "email", title: "Email", name: "email" },
            { "data": "isEnabled", title: "IsEnabled", name: "isEnabled" },
            { "data": "role", title: "Role", name: "role" },
            { "data": "isAssignedToProject", title: "IsAssigned", name: "isAssignedToProject", orderable: false }
        ]
    });
    let contextmenu = $('#projectUserTable').contextMenu({
        selector: 'tr',
        trigger: 'right',
        callback: function (key, options) {
            let row = table.row(options.$trigger);
            switch (key) {
                case 'edit':
                    if (row.data()["isAssignedToProject"] == true) {
                        RemoveUserFromProjectUserProjectPAge(projectId, row.data()["id"]);
                    }
                    else {
                        AddUserToProjectUserProjectPAge(projectId, row.data()["id"]);
                    }
                    break;
                default:
                    break
            }
        },
        items: {
            "edit": { name: "Add/Remove" }
        }
    })
};

function goToUsersofProject(projectId) {
    startLoading();
    $.ajax({
        url: "TaskManager/UsersOfProject",
        data: {
            projectId: projectId
        },
        xhrFields: {
            withCredentials: true
        },
        async: false,
        method: "GET",
        success: function (response) {
            $('#taskManagerContent').html(null);
            $('#taskManagerContent').html(response);
            endLoading();
            usersOfProjectLoad(projectId);
        }
    });
}

function gotToUserCreate() {
    startLoading();
    $.ajax({
        url: "User/Create",
        xhrFields: {
            withCredentials: true
        },
        method: "GET",
        success: function (response) {
            $('#content').html(null);
            $('#content').html(response);
            $("form").removeData("validator");
            $("form").removeData("unobtrusiveValidation");
            $.validator.unobtrusive.parse("form");
            endLoading();
        }
    });

};
function createUser() {
    var form = $("#createUserForm");
    if (form.valid()) {
        startLoading();
        var url = form.attr('action');

        $.ajax({
            url: url,
            xhrFields: {
                withCredentials: true
            },
            data: form.serialize(),
            method: "POST",
            success: function (response) {
                $('#contentDiv').html(null);
                $('#contentDiv').html(response);
                $("form").removeData("validator");
                $("form").removeData("unobtrusiveValidation");
                $.validator.unobtrusive.parse("form");
                userpageLoad();
                endLoading();
            },
            error: function (response) {
                $('#contentDiv').html(null);
                $('#contentDiv').html(response);
                $("form").removeData("validator");
                $("form").removeData("unobtrusiveValidation");
                $.validator.unobtrusive.parse("form");
                endLoading();
            }
        })
    }
};

function modifyUser(userId) {
    var form = $("#edituserForm");
    if (form.valid()) {
        startLoading();
        var url = form.attr('action');

        $.ajax({
            url: url,
            xhrFields: {
                withCredentials: true
            },
            data: form.serialize(),
            method: "POST",
            success: function (response) {
                $('#contentDiv').html(response);
                $("form").removeData("validator");
                $("form").removeData("unobtrusiveValidation");
                $.validator.unobtrusive.parse("form");
                endLoading();
            },
            error: function (response) {
                $('#contentDiv').html(response);
                $("form").removeData("validator");
                $("form").removeData("unobtrusiveValidation");
                $.validator.unobtrusive.parse("form");
                endLoading();
            }
        })
    };
}
function userEditLoad() {
};

function goToEditProject(projectId) {
    startLoading();
    $.ajax({
        url: "TaskManager/EditProject",
        data: {
            projectId: projectId
        },
        xhrFields: {
            withCredentials: true
        },
        method: "GET",
        success: function (response) {
            $('#taskManagerContent').html(null);
            $('#taskManagerContent').html(response);
            $("form").removeData("validator");
            $("form").removeData("unobtrusiveValidation");
            $.validator.unobtrusive.parse("form");
            $('#taskManagerContent').html(response);
            $("form").removeData("validator");
            $("form").removeData("unobtrusiveValidation");
            $.validator.unobtrusive.parse("form");
            $("#datepickerStart").datepicker({
                dateFormat: "yy-mm-dd",
                onSelect: function (datetext) {
                    datetext = datetext + "T00:01"
                    $('#datepickerStart').val(datetext);
                    $('#datepickerStart').attr('value', datetext);
                }
            });

            $("#datepickerEnd").datepicker({
                dateFormat: "yy-mm-dd",
                onSelect: function (datetext) {
                    datetext = datetext + "T00:01"
                    $('#datepickerEnd').val(datetext);
                    $('#datepickerEnd').attr('value', datetext);
                }
            });

            $(".datepicker").each(function () {
                if ($(this).attr('value') !== undefined && $(this).attr('value') != '') {
                    let month = $(this).attr('value').split('/')[0];
                    month = parseInt(month) > 9 ? month : '0' + month;
                    let day = $(this).attr('value').split('/')[1];
                    day = parseInt(day) > 9 ? day : '0' + day;
                    let year = $(this).attr('value').split('/')[2].split(' ')[0];
                    let datetext = year + '-' + month + '-' + day + 'T00:01';
                    console.log(datetext);
                    $(this).val(datetext);
                    $(this).attr('value', datetext);
                }
            });
            endLoading();
        }
    });
}
function goToprojectDetails(projectId) {
    startLoading();
    $.ajax({
        url: "TaskManager/ProjectDetails",
        data: {
            projectId: projectId
        },
        xhrFields: {
            withCredentials: true
        },
        method: "GET",
        success: function (response) {
            $('#taskManagerContent').html(null);
            $('#taskManagerContent').html(response);
            endLoading();
        }
    });
}

function gotToUserEditPage(userId) {
    startLoading();
    $.ajax({
        url: "User/EditUser",
        xhrFields: {
            withCredentials: true
        },
        data: {
            userId: userId
        },
        method: "GET",
        success: function (response) {
            $('#contentDiv').html(response);
            $("form").removeData("validator");
            $("form").removeData("unobtrusiveValidation");
            $.validator.unobtrusive.parse("form");
            endLoading();
        }
    });
}

function gotToUserDetailsPage(userId) {
    startLoading();
    $.ajax({
        url: "User/UserDetails",
        xhrFields: {
            withCredentials: true
        },
        data: {
            userId: userId
        },
        method: "GET",
        success: function (response) {
            $('#contentDiv').html(response);
            endLoading();
        }
    });
}

function goToUserIndex() {
    startLoading();
    $.ajax({
        url: "User/Index",
        xhrFields: {
            withCredentials: true
        },
        method: "GET",
        success: function (response) {
            $('#content').html(null);
            $('#content').html(response);
            endLoading();
            userpageLoad();
        }
    });
}

function loadTaskManagerMenu() {
    startLoading();
    $.ajax({
        url: "TaskManager/TaskManagerMenu",
        xhrFields: {
            withCredentials: true
        },
        method: "GET",
        success: function (response) {
            $('#taskManagerMenu').html(null);
            $('#taskManagerMenu').html(response);
            endLoading();
        }
    });
}

function editTogle() {
    let editMode = document.querySelector('#editCheckBox').checked;
    console.log(editMode);

    let elements = document.getElementsByClassName('visibleIfEditClass');
    [].forEach.call(elements, function (el) {
        if (editMode) {
            el.style.display = "block";
        }
        else {
            el.style.display = "none";
        }
    });
    elements = document.getElementsByClassName('writeIfEditClass');
    [].forEach.call(elements, function (el) {
        el.disabled = !editMode;
    });
}

function RemoveUserFromTask(userId, taskId) {
    startLoading();
    $.ajax({
        url: "/TaskManager/RemoveUserFromTask?taskId=" + taskId + "&userId=" + userId,
        type: "POST",
        success: function (result) {
            endLoading();
            gotToUserEditPage(userId);
        }
    });
}
function RemoveUserFromTaskProject(userId, taskId) {
    startLoading();
    $.ajax({
        url: "/TaskManager/RemoveUserFromTask?taskId=" + taskId + "&userId=" + userId,
        type: "POST",
        success: function (result) {
            endLoading();
            goToTaskEdit(taskId);
        }
    });
}
function RemoveUserFromProject(projectId, userId) {
    startLoading();
    $.ajax({
        url: "/TaskManager/RemoveUserFromProject?projectId=" + projectId + "&userId=" + userId,
        type: "POST",
        success: function (result) {
            endLoading();
            gotToUserEditPage(userId);
        }
    });
}

function AddUserToProjectUserProjectPAge(projectId, userId) {
    startLoading();
    $.ajax({
        url: "/TaskManager/AddOneUserToProject?projectId=" + projectId + "&userId=" + userId,
        type: "POST",
        success: function (result) {
            endLoading();
            goToUsersofProject(projectId);
        }
    });
}

function RemoveUserFromProjectUserProjectPAge(projectId, userId) {
    startLoading();
    $.ajax({
        url: "/TaskManager/RemoveUserFromProject?projectId=" + projectId + "&userId=" + userId,
        type: "POST",
        success: function (result) {
            endLoading();
            goToUsersofProject(projectId);
        }
    });
}

function RemoveUserFromProjectProjectPage(projectId, userId) {
    startLoading();
    $.ajax({
        url: "/TaskManager/RemoveUserFromProject?projectId=" + projectId + "&userId=" + userId,
        type: "POST",
        success: function (result) {
            endLoading();
            goToEditProject(projectId);
        }
    });
}
function goToTaskManagerIndex() {
    startLoading();
    $.ajax({
        url: "TaskManager/Index",
        xhrFields: {
            withCredentials: true
        },
        method: "GET",
        success: function (response) {
            $('#contentDiv').html(null);
            $('#contentDiv').html(response);
            endLoading();
            loadTaskManagerMenu();
        }
    });
}
function goToTaskManagerCreateProject() {
    startLoading();
    $.ajax({
        url: "TaskManager/CreateProject",
        xhrFields: {
            withCredentials: true
        },
        method: "GET",
        success: function (response) {
            $('#taskManagerContent').html(null);
            $('#taskManagerContent').html(response);
            $("form").removeData("validator");
            $("form").removeData("unobtrusiveValidation");
            $.validator.unobtrusive.parse("form");
            endLoading();
        }
    });
}
function createProject() {
    var form = $("#createProjectForm");
    if (form.valid()) {
        startLoading();
        var url = form.attr('action');

        $.ajax({
            url: url,
            xhrFields: {
                withCredentials: true
            },
            data: form.serialize(),
            method: "POST",
            success: function (response) {
                $('#taskManagerContent').html(response);
                $("form").removeData("validator");
                $("form").removeData("unobtrusiveValidation");
                $.validator.unobtrusive.parse("form");
                loadTaskManagerMenu();
                endLoading();
            },
            error: function (response) {
                $('#taskManagerContent').html(response);
                $("form").removeData("validator");
                $("form").removeData("unobtrusiveValidation");
                $.validator.unobtrusive.parse("form");
                loadTaskManagerMenu();
                endLoading();
            }
        })
    };
}
function taskManagerLoadProject(projectId) {
    let table = $('#example').DataTable({
        processing: true,
        serverSide: true,
        "ajax": {
            url: "TaskManager/ReturnData",
            data: {
                projectId: projectId
            },
            type: "POST"
        },
        "columns": [
            { data: "id", title: "Id", name: "id", visible: false },
            { data: "name", title: "Name", name: "name" },
            { data: "description", title: "Description", name: "description" },
            { data: "taskType", title: "TaskType", name: "taskType" },
            { data: "priority", title: "Priority", name: "priority" },
            { data: "taskState", title: "State", name: "taskState" }
        ],
        'rowCallback': function (row, data, index) {
            if (data['priority'] == 'Urgent') {
                $(row).find('td:eq(3)').css('color', '#BA0021');
            }
            if (data['priority'] == 'High') {
                $(row).find('td:eq(3)').css('color', '#ffa500');
            }
            if (data['priority'] == 'Medium') {
                $(row).find('td:eq(3)').css('color', '#008000');
            }
            $(row).find('td:eq(3)').html('<h6>' + data['priority'] + '</h6>');
        }
    });
    let contextmenu = $('#example').contextMenu({
        selector: 'tr',
        trigger: 'right',
        callback: function (key, options) {
            var row = table.row(options.$trigger)
            switch (key) {
                case 'edit':
                    goToTaskEdit(row.data()["id"]);
                    break;
                case 'details':
                    goToTaskDetails(row.data()["id"]);
                    break;
                case 'delete':
                    deleteTask(row.data()["id"]);
                    table.ajax.reload();
                    break;
                default:
                    break
            }
        },
        items: {
            "edit": { name: "Edit" },
            "details": { name: "Details" },
            "delete": { name: "Delete" },
        }
    })
}

function deleteTask(taskId) {
    startLoading();
    $.ajax({
        url: "/TaskManager/DeleteTask?taskId=" + taskId,
        type: "POST",
        async: false,
        done: function (result) {
            endLoading();
            console.log(result);
        }
    });
}

function goToTaskDetails(taskId) {
    startLoading();
    $.ajax({
        url: "TaskManager/TaskDetails",
        data: {
            taskId: taskId
        },
        xhrFields: {
            withCredentials: true
        },
        method: "GET",
        success: function (response) {
            $('#taskManagerContent').html(null);
            $('#taskManagerContent').html(response);
            endLoading();
        }
    });
}

function goToTaskEdit(taskId) {
    startLoading();
    $.ajax({
        url: "TaskManager/EditTask",
        data: {
            taskId: taskId
        },
        async: false,
        xhrFields: {
            withCredentials: true
        },
        method: "GET",
        success: function (response) {
            $('#taskManagerContent').html(null);
            $('#taskManagerContent').html(response);
            $("form").removeData("validator");
            $("form").removeData("unobtrusiveValidation");
            $.validator.unobtrusive.parse("form");
            $("#datepickerStart").datepicker({
                dateFormat: "yy-mm-dd",
                onSelect: function (datetext) {
                    datetext = datetext + "T00:01"
                    $('#datepickerStart').val(datetext);
                    $('#datepickerStart').attr('value', datetext);
                }
            });
            $("#datepickerEnd").datepicker({
                dateFormat: "yy-mm-dd",
                onSelect: function (datetext) {
                    datetext = datetext + "T00:01"
                    $('#datepickerEnd').val(datetext);
                    $('#datepickerEnd').attr('value', datetext);
                }
            });

            $(".datepicker").each(function () {
                if ($(this).attr('value') !== undefined && $(this).attr('value') != '') {
                    let month = $(this).attr('value').split('/')[0];
                    month = parseInt(month) > 9 ? month : '0' + month;
                    let day = $(this).attr('value').split('/')[1];
                    day = parseInt(day) > 9 ? day : '0' + day;
                    let year = $(this).attr('value').split('/')[2].split(' ')[0];
                    let datetext = year + '-' + month + '-' + day + 'T00:01';
                    console.log(datetext);
                    $(this).val(datetext);
                    $(this).attr('value', datetext);
                }
            });


            endLoading();
        }
    });
}

function goToProject(projectId) {
    startLoading();
    $.ajax({
        url: "TaskManager/IndexContent",
        data: {
            projectId: projectId
        },
        xhrFields: {
            withCredentials: true
        },
        method: "GET",
        success: function (response) {
            $('#taskManagerContent').html(null);
            $('#taskManagerContent').html(response);
            endLoading();
            taskManagerLoadProject(projectId);
        }
    });
}
function goToCreateTask(projectId) {
    startLoading();
    $.ajax({
        url: "TaskManager/CreateTask",
        data: {
            projectId: projectId
        },
        xhrFields: {
            withCredentials: true
        },
        method: "GET",
        success: function (response) {
            $('#taskManagerContent').html(null);
            $('#taskManagerContent').html(response);
            $("form").removeData("validator");
            $("form").removeData("unobtrusiveValidation");
            $.validator.unobtrusive.parse("form");
            endLoading();
        }
    });
}
function createTask(projectId) {
    var form = $("#createTaskForm");
    if (form.valid()) {
        startLoading();
        var url = form.attr('action');

        $.ajax({
            url: url,
            xhrFields: {
                withCredentials: true
            },
            data: form.serialize(),
            method: "POST",
            success: function (response) {
                $('#taskManagerContent').html(response);
                $("form").removeData("validator");
                $("form").removeData("unobtrusiveValidation");
                $.validator.unobtrusive.parse("form");
                taskManagerLoadProject(projectId);
                endLoading();
            },
            error: function (response) {
                $('#taskManagerContent').html(response);
                $("form").removeData("validator");
                $("form").removeData("unobtrusiveValidation");
                $.validator.unobtrusive.parse("form");
                endLoading();
            }
        })
    };
}

function modifyProject(projectId) {
    var form = $("#modifyProjectForm");
    if (form.valid()) {
        startLoading();
        var url = form.attr('action');

        $.ajax({
            url: url,
            xhrFields: {
                withCredentials: true
            },
            data: form.serialize(),
            method: "POST",
            success: function (response) {
                $('#taskManagerContent').html(response);
                $("form").removeData("validator");
                $("form").removeData("unobtrusiveValidation");
                $.validator.unobtrusive.parse("form");
                endLoading();
                loadTaskManagerMenu();
                $('#taskManagerContent').html(response);
                $("form").removeData("validator");
                $("form").removeData("unobtrusiveValidation");
                $.validator.unobtrusive.parse("form");
                $("#datepickerStart").datepicker({
                    dateFormat: "yy-mm-dd",
                    onSelect: function (datetext) {
                        datetext = datetext + "T00:01"
                        $('#datepickerStart').val(datetext);
                        $('#datepickerStart').attr('value', datetext);
                    }
                });

                $("#datepickerEnd").datepicker({
                    dateFormat: "yy-mm-dd",
                    onSelect: function (datetext) {
                        datetext = datetext + "T00:01"
                        $('#datepickerEnd').val(datetext);
                        $('#datepickerEnd').attr('value', datetext);
                    }
                });

                $(".datepicker").each(function () {
                    if ($(this).attr('value') !== undefined && $(this).attr('value') != '') {
                        let month = $(this).attr('value').split('/')[0];
                        month = parseInt(month) > 9 ? month : '0' + month;
                        let day = $(this).attr('value').split('/')[1];
                        day = parseInt(day) > 9 ? day : '0' + day;
                        let year = $(this).attr('value').split('/')[2].split(' ')[0];
                        let datetext = year + '-' + month + '-' + day + 'T00:01';
                        console.log(datetext);
                        $(this).val(datetext);
                        $(this).attr('value', datetext);
                    }
                });
            },
            error: function (response) {
                $('#taskManagerContent').html(response);
                $("form").removeData("validator");
                $("form").removeData("unobtrusiveValidation");
                $.validator.unobtrusive.parse("form");
                endLoading();
                loadTaskManagerMenu();
                $('#taskManagerContent').html(response);
                $("form").removeData("validator");
                $("form").removeData("unobtrusiveValidation");
                $.validator.unobtrusive.parse("form");
                $("#datepickerStart").datepicker({
                    dateFormat: "yy-mm-dd",
                    onSelect: function (datetext) {
                        datetext = datetext + "T00:01"
                        $('#datepickerStart').val(datetext);
                        $('#datepickerStart').attr('value', datetext);
                    }
                });

                $("#datepickerEnd").datepicker({
                    dateFormat: "yy-mm-dd",
                    onSelect: function (datetext) {
                        datetext = datetext + "T00:01"
                        $('#datepickerEnd').val(datetext);
                        $('#datepickerEnd').attr('value', datetext);
                    }
                });

                $(".datepicker").each(function () {
                    if ($(this).attr('value') !== undefined && $(this).attr('value') != '') {
                        let month = $(this).attr('value').split('/')[0];
                        month = parseInt(month) > 9 ? month : '0' + month;
                        let day = $(this).attr('value').split('/')[1];
                        day = parseInt(day) > 9 ? day : '0' + day;
                        let year = $(this).attr('value').split('/')[2].split(' ')[0];
                        let datetext = year + '-' + month + '-' + day + 'T00:01';
                        console.log(datetext);
                        $(this).val(datetext);
                        $(this).attr('value', datetext);
                    }
                });
            }
        })
    };
}

function modifyTask(taskId) {
    var form = $("#modifyTaskForm");
    if (form.valid()) {
        startLoading();
        var url = form.attr('action');

        $.ajax({
            url: url,
            xhrFields: {
                withCredentials: true
            },
            data: form.serialize(),
            method: "POST",
            success: function (response) {
                $('#taskManagerContent').html(response);
                $("form").removeData("validator");
                $("form").removeData("unobtrusiveValidation");
                $.validator.unobtrusive.parse("form");
                $("#datepickerStart").datepicker({
                    dateFormat: "yy-mm-dd",
                    onSelect: function (datetext) {
                        datetext = datetext + "T00:01"
                        $('#datepickerStart').val(datetext);
                        $('#datepickerStart').attr('value', datetext);
                    }
                });

                $("#datepickerEnd").datepicker({
                    dateFormat: "yy-mm-dd",
                    onSelect: function (datetext) {
                        datetext = datetext + "T00:01"
                        $('#datepickerEnd').val(datetext);
                        $('#datepickerEnd').attr('value', datetext);
                    }
                });


                $(".datepicker").each(function () {
                    if ($(this).attr('value') !== undefined && $(this).attr('value') != '') {
                        let month = $(this).attr('value').split('/')[0];
                        month = parseInt(month) > 9 ? month : '0' + month;
                        let day = $(this).attr('value').split('/')[1];
                        day = parseInt(day) > 9 ? day : '0' + day;
                        let year = $(this).attr('value').split('/')[2].split(' ')[0];
                        let datetext = year + '-' + month + '-' + day + 'T00:01';
                        console.log(datetext);
                        $(this).val(datetext);
                        $(this).attr('value', datetext);
                    }
                });
                endLoading();
            }
        })
    };
}

function adduserToProject(projectId) {
    var form = $("#adduserToProjectForm");
    if (form.valid()) {
        startLoading();
        var url = form.attr('action');

        $.ajax({
            url: url,
            xhrFields: {
                withCredentials: true
            },
            data: form.serialize(),
            method: "POST",
            success: function (response) {
                endLoading();
                goToEditProject(projectId);
            }
        })
    };
}

function addUserToTask(taskId) {
    var form = $("#addUserToTaskForm");
    if (form.valid()) {
        startLoading();
        var url = form.attr('action');

        $.ajax({
            url: url,
            xhrFields: {
                withCredentials: true
            },
            data: form.serialize(),
            method: "POST",
            success: function (response) {
                endLoading();
                goToTaskEdit(taskId);
            }
        })
    };
}
