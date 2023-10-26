let filter = $('#projectsInput').val();
$('.list-group').empty();

$.ajax({
    url: "TaskManager/GetAllProjects",
    type: "POST",
    data: filter,
    success: function (response) {
        if (response) {
            response.forEach(project => {
                $('#projectItem').empty();
                $('#projectItem').append(
                    `<li class="nav-item">
                        <a href="#" class="nav-link">
                            <div class="d-flex justify-space-between">
                                <i class="bi bi-boxes mr-1 mt-1"></i>
                                <h6 class="mt-1">
                                    ${project.name}
                                </h6>
                                <i class="bi bi-three-dots-vertical ml-auto mt-1 mb-2 text-center"></i>

                            </div>
                            <small>Started ${project.startDate}</small>
                        </a>
                    </li>`
                );
            });
        }
    },
    error: function (jqXHR, textStatus, errorThrown) {
        $('#projectItem').empty();
        $('#projectItem').append(
            `<li class="nav-item">
                <a href="#" class="nav-link">
                    <i class="bi bi-bookmark-fill"></i>
                    <p>
                        <span class="right badge badge-danger">Error fetching data</span>
                    </p>
                </a>
            </li>`
        );
    }
});

$('#filterProjects').on('click', function () {
    let filter = $('#projectsInput').val();

    $.ajax({
        url: "TaskManager/GetAllProjects&filter="+filter,
        type: "POST",
        success: function (response) {
            if (response) {
                response.forEach(project => {
                    $('.list-group').empty();
                    $('.list-group').append(
                        `<a href="#" class="list-group-item">
                            <div class="d-flex justify-space-between">
                                <i class="nav-icon bi bi-arrow-down-right-square"></i>
                                <h6 class="mt-1">
                                    ${project.name}
                                </h6>
                                <i class="bi bi-three-dots-vertical ml-auto mt-1 mb-2 text-center"></i>

                            </div>
                            <small>Started ${project.startDate}</small>
                        </a>`
                    );
                });
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            $('#projectItem').empty();
            $('#projectItem').append(
                `<li class="nav-item">
                    <a href="#" class="nav-link">
                        <i class="nav-icon bi bi-arrow-down-right-square"></i>
                        <p>
                            Simple Link
                            <span class="right badge badge-danger">Error fetching data</span>
                        </p>
                    </a>
                </li>`
            );
        }
    });
});
