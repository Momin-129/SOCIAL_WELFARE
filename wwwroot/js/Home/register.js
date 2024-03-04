$(document).ready(function () {
  $("#registerButton").remove();

  let showPassword = false;
  let showConfirmPassword = false;

  $("input").on("focus", function () {
    $(this).css("outline", "none").css("box-shadow", "none");
  });
  $("button").on("focus", function () {
    $(this)
      .css("outline", "none")
      .css("box-shadow", "none")
      .addClass("bg-success border-0");
  });

  $("#togglePassword").click(function () {
    showPassword = true;
    var passwordInput = $("#password");

    // Toggle the password visibility
    passwordInput.prop("type", function (index, currentType) {
      return currentType === "password" ? "text" : "password";
    });
    $("#togglePassword").toggleClass("bi-eye bi-eye-slash");
  });

  $("#toggleConfirmPassword").click(function () {
    showConfirmPassword = true;
    var passwordInput = $("#confirmpassword");

    // Toggle the password visibility
    passwordInput.prop("type", function (index, currentType) {
      return currentType === "password" ? "text" : "password";
    });
    $("#toggleConfirmPassword").toggleClass("bi-eye bi-eye-slash");
  });

  function ErrorSpan(id, msg) {
    if ($("#" + id + "Msg").length) {
      $("#" + id + "Msg").text(msg);
    } else {
      const newSpan = $("<span>")
        .attr({
          id: `${id}Msg`,
          class: "text-danger fw-bold d-flex justify-content-center",
          style: "white-space: pre-line",
        })
        .text(msg);
      if (id == "password" || id == "confirmpassword") {
        $("#" + id)
          .parent()
          .after(newSpan);
      } else $("#" + id).after(newSpan);
    }
  }

  $("#username").on("blur", function () {
    const value = $(this).val();
    if (/[\s]/.test(value)) {
      ErrorSpan("username", "Username should not contain space.");
    } else {
      if ($("#usernameMsg").length) {
        $("#usernameMsg").text("");
      }
    }
  });

  $("#email").on("blur", function () {
    const value = $(this).val();
    if (/[\s]/.test(value)) {
      ErrorSpan("email", "Email should not contain space.");
    } else {
      if ($("#emailMsg").length) {
        $("#emailMsg").text("");
      }
    }
  });

  $("#password").on("blur", function () {
    if (!showPassword) {
      const value = $(this).val();
      let msg = "";
      if (value.length < 8) {
        msg += "Password should contain atleast 8 characters.\n";
      }
      if (!/[a-z]/.test(value)) {
        msg += "Password should contain atleast one lowercase charater.\n";
      }
      if (!/[A-Z]/.test(value)) {
        msg += "Password should contain atleast one uppercase charater.\n";
      }
      if (!/[0-9]/.test(value)) {
        msg += "Password should contain atleast one digit.\n";
      }
      if (!/[@$&#]/.test(value)) {
        msg += "Password should contain atleast one of these charaters @$&#.\n";
      }

      if (msg.length > 0) {
        ErrorSpan("password", msg);
      } else {
        if ($("#passwordMsg").length) {
          $("#passwordMsg").text("");
        }
      }
    }
    showPassword = false;
  });

  $("#confirmpassword").on("blur", function () {
    if (!showConfirmPassword) {
      const value = $(this).val();
      if (value != $("#password").val()) {
        ErrorSpan(
          "confirmpassword",
          "Confirm Password is not same as Password."
        );
      } else {
        if ($("#confirmpasswordMsg").length) {
          $("#confirmpasswordMsg").text("");
        }
      }
    }
    showConfirmPassword = false;
  });

  $("#registerForm").submit(function (e) {
    e.preventDefault();
    const username = $("#username").val();
    const email = $("#email").val();
    const password = $("#password").val();
    const confirmpassword = $("#confirmpassword").val();

    const formData = new FormData(this);
    console.log(formData);
    if (
      username.length == 0 ||
      email.length == 0 ||
      password.length == 0 ||
      confirmpassword.length == 0
    ) {
      $("#registrationError").text("All fields are required.");
    } else {
      fetch("/Home/Register", {
        method: "POST",
        body: formData,
      })
        .then((response) => response.json())
        .then((data) => {
          console.log(data);
          if (data.success) {
            window.location.href = data.url + "?userId=" + data.userId;
          }
        })
        .catch((error) => console.error("Error:", error));
      $("#registrationError").text("");
    }
  });
  // End
});
