$(document).ready(function () {
  drawCaptcha();

  $("#loginButton").remove();

  // Refresh CAPTCHA on button click
  $("#refreshCaptcha").click(function () {
    drawCaptcha();
  });

  $("#togglePassword").click(function () {
    var passwordInput = $("#password");

    // Toggle the password visibility
    passwordInput.prop("type", function (index, currentType) {
      return currentType === "password" ? "text" : "password";
    });
    $("#togglePassword").toggleClass("bi-eye bi-eye-slash");
  });

  $("#captchaInput").blur(function () {
    if (!($("#captchaInput").val() == window.captchaText)) {
      $("#captchaEroor").text("Incorrect Captcha");
    } else $("#captchaEroor").text("");
  });

  $("#loginForm").submit(function (e) {
    e.preventDefault();

    if (!($("#captchaInput").val() == window.captchaText)) {
      $("#captchaEroor").text("Incorrect Captcha");
    } else if (
      $("#username").val().length == 0 &&
      $("#password").val().length == 0
    ) {
      alert("Login ID or Password can't be empty.");
    } else {
      const dataToSend = {
        Username: $("#username").val(),
        Password: $("#password").val(),
      };
      $.ajax({
        url: "/Home/Login", // Replace with your login action URL
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(dataToSend),
        success: function (data) {
          if (data.status) {
            window.location.href = data.url;
          } else {
            $("#loginError").text("");
            setTimeout(() => $("#loginError").text(data.message), 500);
          }
        },
        error: function (error) {
          alert("An error occurred during login.", error);
        },
      });
    }
  });

  $("input").on("focus", function () {
    $(this).css("outline", "none").css("box-shadow", "none");
  });
  $("button").on("focus", function () {
    $(this)
      .css("outline", "none")
      .css("box-shadow", "none")
      .addClass("bg-transparent border-0");
  });
});

function generateCaptcha() {
  const characters =
    "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
  let captchaText = "";

  for (let i = 0; i < 6; i++) {
    captchaText += characters.charAt(
      Math.floor(Math.random() * characters.length)
    );
  }

  return captchaText;
}

function drawCaptcha() {
  const captchaValue = generateCaptcha();
  const fonts = ["cursive"];

  let html = captchaValue
    .split("")
    .map((char) => {
      const rotate = -20 + Math.trunc(Math.random() * 30);
      const font = Math.trunc(Math.random() * fonts.length);
      return `<span
            style="
            transform:rotate(${rotate}deg);
            font-family:${font[font]};
            user-select:none;
            "
           >${char} </span>`;
    })
    .join("");

  $("#captchaCanvas").html(html);

  window.captchaText = captchaValue;
}
