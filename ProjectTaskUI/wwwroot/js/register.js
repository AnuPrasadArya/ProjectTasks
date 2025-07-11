document.getElementById("registerForm").addEventListener("submit", async function (e) {
    e.preventDefault();
    const username = document.getElementById("username").value;
    const password = document.getElementById("password").value;

    const response = await fetch("https://localhost:7010/api/auth/register", {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ username, password })
    });

    if (response.ok) {
        document.getElementById("registerSuccess").innerText = "Registered successfully. Please login.";
        document.getElementById("registerSuccess").style.display = "block";
    } else {
        document.getElementById("registerError").innerText = "Error during registration.";
        document.getElementById("registerError").style.display = "block";
    }
});
