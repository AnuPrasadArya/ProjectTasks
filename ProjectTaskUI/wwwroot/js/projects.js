$(document).ready(function () {    
    loadProjects();
});

function showProjectForm() {
    $("#projectForm").toggle();
}
async function loadProjects() {
    const token = localStorage.getItem("accessToken");
    const UserId = parseInt(localStorage.getItem("userId"));    
    const res = await fetch("https://localhost:7125/api/Project/GetProject", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Authorization": "Bearer " + token
        },
        body: JSON.stringify({ UserId })
    });

    if (res.ok) {
        const projects = await res.json();
        const body = $("#projectsBody");
        body.empty();

        projects.forEach(project => {
            const row = `
                <tr>
                    <td>${project.id}</td>
                    <td><input type="text" class="form-control" id="name_${project.id}" value="${project.name}" /></td>
                    <td><textarea class="form-control" id="desc_${project.id}">${project.description}</textarea></td>
                    <td>
                        <button class="btn btn-sm btn-primary" onclick="updateProject(${project.id})">Update</button>
                        <button class="btn btn-sm btn-danger" onclick="deleteProject(${project.id})">Delete</button>
                    </td>
                </tr>
            `;
            body.append(row);
        });

        $("#projectsTable").show();
    } else {
        alert("Failed to load projects.");
    }
}
async function createProject() {
    const name = document.getElementById("projectName").value;
    const description = document.getElementById("projectDesc").value;
    const token = localStorage.getItem("accessToken");
    const isDelete = false;
    const UserId = parseInt(localStorage.getItem("userId"));
    const response = await fetch("https://localhost:7125/api/Project/CreateProject", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Authorization": "Bearer " + token
        },
        body: JSON.stringify({ name, description, isDelete, UserId })
    });

    if (response.ok) {
        alert("Project created!");
        location.reload();
    } else {
        alert("Error creating project.");

    }
}

async function updateProject(id) {
    const name = $(`#name_${id}`).val().trim();
    const description = $(`#desc_${id}`).val().trim();
    const token = localStorage.getItem("accessToken");
    const userId = parseInt(localStorage.getItem("userId"));
    const isDelete = false;

    const response = await fetch(`https://localhost:7125/api/Project/UpdateProject`, {
        method: "PUT",
        headers: {
            "Content-Type": "application/json",
            "Authorization": "Bearer " + token
        },
        body: JSON.stringify({ id, name, description, isDelete, userId })
    });

    if (response.ok) {
        alert("Project updated!");
        location.reload();
    } else {
        alert("Error updating project.");
    }
}
async function deleteProject(id) {
    const confirmDelete = confirm("Are you sure you want to delete this project?");
    if (!confirmDelete) return;

    const token = localStorage.getItem("accessToken");
    const userId = parseInt(localStorage.getItem("userId"));
    const response = await fetch(`https://localhost:7125/api/Project/DeleteProject`, {
        method: "DELETE",
        headers: {
            "Content-Type": "application/json",
            "Authorization": "Bearer " + token
        },
        body: JSON.stringify({ id, userId })
    });

    if (response.ok) {
        alert("Project deleted!");
        location.reload();
    } else {
        alert("Error deleting project.");
    }
}

function goToTaskPage() {
    window.location.href = "/Home/Tasks"; 
}


