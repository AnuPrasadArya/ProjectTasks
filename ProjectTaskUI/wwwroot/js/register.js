document.getElementById("registerForm").addEventListener("submit", async function (e) {
    e.preventDefault();
    const username = document.getElementById("username").value;
    const password = document.getElementById("password").value;

    const response = await fetch("https://localhost:7125/api/Register/NewUser", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ username, password })
    });

    if (response.ok) {
        alert("User Registered Successfully");
        window.location.href = "/Home/Login";
    } else {
        document.getElementById("registerError").innerText = "Error during registration.";
        document.getElementById("registerError").style.display = "block";
    }
});
