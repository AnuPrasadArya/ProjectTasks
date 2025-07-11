function showProjectForm() {
    document.getElementById("projectForm").style.display = "block";
}

async function createProject() {
    const name = document.getElementById("projectName").value;
    const desc = document.getElementById("projectDesc").value;
    const token = localStorage.getItem("accessToken");

    const response = await fetch("https://localhost:7010/api/projects", {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
            "Authorization": "Bearer " + token
        },
        body: JSON.stringify({ name, description: desc })
    });

    if (response.ok) {
        alert("Project created!");
        loadProjects();
    } else {
        alert("Error creating project.");
    }
}

async function loadProjects() {
    const token = localStorage.getItem("accessToken");

    const res = await fetch("https://localhost:7010/api/projects", {
        headers: { "Authorization": "Bearer " + token }
    });

    if (res.ok) {
        const projects = await res.json();
        const list = document.getElementById("projectsList");
        list.innerHTML = "";
        projects.forEach(p => {
            const li = document.createElement("li");
            li.className = "list-group-item";
            li.innerText = p.name;
            list.appendChild(li);
        });
    } else {
        alert("Error loading projects");
    }
}

document.addEventListener("DOMContentLoaded", loadProjects);
