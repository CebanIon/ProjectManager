$(document).ready(function () {
    searchProjects("");

    $('#createProjectButton').on('click', function(){
        viewCreateProject();
    });
});

$('#projectsInput').on('input', function () {
    $('.sidebar-search-results').remove();
    let filter = $(this).val();
    if (!filter){
        searchProjects("");
    }
});
$('#projectsInput').on('keypress', function (e) {
    if (e.which === 13) { 
        e.preventDefault();
        $('#filterProjects').click();
    }
});

$('#filterProjects').on('click', function () {
    let filter = $('#projectsInput').val();
    if (filter !== "") {
        searchProjects(filter);
    }
});

function searchProjects(filter) {
    $('.list-group').empty();
    let items = [];
    $.ajax({
        url: "TaskManager/GetAllProjects",
        data: { filter: filter },
        type: "POST",
        success: function (response) {
            if (response && response.length > 0) {
                response.forEach((project, index) => {
                    let dropDirection = (index === response.length - 1 && index > 3) ? 'dropup' : '';
                    items.push(
                        `<li class="nav-item" data-id=${project.id}>
                            <a class="nav-link" role="button" onClick="viewProject(${project.id}, this)">
                                <div class="d-flex justify-space-between">
                                    <i class="bi bi-boxes nav-icon"></i>
                                    <h6 class="mt-1">
                                        ${project.name}
                                    </h6>
                                    <div class="ml-auto mt-1 mb-2 text-center">
                                        <div class="btn-group dropdown ${dropDirection}">
                                            <i class="bi bi-three-dots-vertical" role="button" id="${project.name}" data-bs-toggle="dropdown" aria-expanded="false"></i>
                                            <ul class="dropdown-menu project-menu" aria-labelledby="${project.name}" style="right: 0!important;left: unset; z-index:1000; min-width: auto; padding: 0.5em;">
                                                <li><button class="dropdown-item" onClick="viewProject(${project.id})" type="button">View</button></li>
                                                <li><button class="dropdown-item" onClick="editProject(${project.id})" type="button">Edit</button></li>
                                            </ul>
                                        </div>
                                    </div>
                                </div>
                                <small>Started ${project.startDate}</small>
                            </a>
                        </li>`);
                        //<i class="bi bi-three-dots-vertical" role="button" id="${project.name}" data-bs-toggle="dropdown" aria-expanded="false"></i>
                });
                updateSearchResults(items);
                $('.bi-three-dots-vertical').dropdown();

            } else {
                showNoElementFound();
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            showNoElementFound();
        }
    })
}

$(document).on('click', '.bi-three-dots-vertical', function (e) {
    let dropdownmenu = $(this).next('.project-menu');
    dropdownmenu.addClass('show');
    e.stopPropagation();
});

$(document).on('click', function (e) {
    if (!$(e.target).closest('.btn-group').length) {
        $('.project-menu').removeClass('show');
    }
});

function updateSearchResults(items) {
    if (items.length > 0) {
        $('#projectItem').html(items.join(''));
    } else {
        showNoElementFound();
    }
}
function showNoElementFound() {
    $('#projectItem').html('<a href="#" class="list-group-item"><div class="search-title">No element found!</div><div class="search-path"></div></a>');
}
