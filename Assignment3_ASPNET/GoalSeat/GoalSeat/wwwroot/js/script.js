function validateLogin() {
    var email = document.getElementById("loginEmail").value;
    var password = document.getElementById("loginPassword").value;

    if (email === "" || password === "") {
        alert("Please fill in all login fields.");
        return false;
    }

    alert("Login successful. Welcome to GoalSeat!");
    return false;
}

function validateSignup() {
    var name = document.getElementById("signupName").value;
    var email = document.getElementById("signupEmail").value;
    var phone = document.getElementById("signupPhone").value;
    var password = document.getElementById("signupPassword").value;
    var confirmPassword = document.getElementById("confirmPassword").value;

    if (name === "" || email === "" || phone === "" || password === "" || confirmPassword === "") {
        alert("Please fill in all signup fields.");
        return false;
    }

    if (password !== confirmPassword) {
        alert("Passwords do not match.");
        return false;
    }

    alert("Account created successfully. Welcome to GoalSeat!");
    return false;
}

function calculatePrice() {
    var matchSelect = document.getElementById("matchName");
    var ticketTypeSelect = document.getElementById("ticketType");
    var count = document.getElementById("ticketCount").value;
    var priceResult = document.getElementById("priceResult");

    var basePrice = matchSelect.options[matchSelect.selectedIndex].dataset.price;
    var multiplier = ticketTypeSelect.options[ticketTypeSelect.selectedIndex].dataset.multiplier;

    if (!basePrice || !multiplier || count === "") {
        priceResult.innerHTML = "Ticket price will appear here.";
        priceResult.style.color = "white";
        return;
    }

    if (Number(count) <= 0) {
        priceResult.innerHTML = "Number of tickets must be at least 1.";
        priceResult.style.color = "#FF2882";
        return;
    }

    var total = Number(basePrice) * Number(multiplier) * Number(count);

    priceResult.innerHTML = "Total Price: " + total + " SAR";
    priceResult.style.color = "#00FF87";
}

function bookTicket() {
    var name = document.getElementById("customerName").value;
    var email = document.getElementById("customerEmail").value;
    var match = document.getElementById("match").value;
    var ticketType = document.getElementById("ticketType").value;
    var count = document.getElementById("ticketCount").value;
    var priceResult = document.getElementById("priceResult");

    if (name === "" || email === "" || match === "" || ticketType === "" || count === "") {
        alert("Please fill in all booking fields.");
        return false;
    }

    if (count <= 0) {
        alert("Number of tickets must be at least 1.");
        return false;
    }

    var total = match * ticketType * count;

    priceResult.innerHTML = "Booking confirmed for " + name + ". Total price: " + total + " SAR.";
    priceResult.style.color = "#00FF87";

    alert("Ticket booked successfully.");
    return false;
}

function resetMessage() {
    var priceResult = document.getElementById("priceResult");
    priceResult.innerHTML = "Ticket price will appear here.";
    priceResult.style.color = "white";
}