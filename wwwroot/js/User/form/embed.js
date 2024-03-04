$(document).ready(function () {
  $.ajax({
    url: "/User/GetDistricts",
    method: "GET",
    dataType: "json",
    success: function (data) {
      if (data.success) {
        let districtList = data.districts;
        $("#District").append(
          `<option value="Please Select">Please Select</option>`
        );
        $("#PresentDistrict").append(
          `<option value="Please Select">Please Select</option>`
        );
        $("#PermanentDistrict").append(
          `<option value="Please Select">Please Select</option>`
        );

        districtList.map((item) => {
          $("#District").append(
            `<option value="${item.uuid}" id="${
              item.districtName
            }">${item.districtName.toUpperCase()}</option>`
          );
          $("#PresentDistrict").append(
            `<option value="${item.uuid}" id="${
              item.districtName
            }">${item.districtName.toUpperCase()}</option>`
          );
          $("#PermanentDistrict").append(
            `<option value="${item.uuid}" id="${
              item.districtName
            }">${item.districtName.toUpperCase()}</option>`
          );
        });
      } else {
        console.error("Error: " + data.error);
      }
    },
    error: function (error) {
      console.error("AJAX Error: " + error);
    },
  });

  let selectedSelects = $(
    'select[id="District"], select[id="PresentDistrict"], select[id="PermanentDistrict"]'
  );

  // To update Tehsils based on District
  selectedSelects.each(function () {
    $(this).on("change", function () {
      var id = $(this).attr("id");
      var selectedValue = $(this).find("option:selected").attr("id");
      updateDistrict(id, selectedValue);
    });
  });

  //  To update pension type
  $("#pensionType").change(function () {
    let selectedValue = $(this).val();
    updatePensionType(selectedValue);
  });

  // to copy the present address to permanent address
  $("#SameAsPresent").change(function () {
    if ($(this).prop("checked")) {
      copyPresentToPermanent();
    } else {
      empytPermanent();
    }
  });

  // to set age
  $("#DOB").change(function () {
    const dob = new Date($(this).val());
    const currentDate = new Date();

    const timeDifference = currentDate - dob;

    const ageInYears = Math.floor(
      timeDifference / (365.25 * 24 * 60 * 60 * 1000)
    );

    $("#Age").val(ageInYears);
    $("#hiddenAge").val(ageInYears);
  });

  $("#isPreviousBank").change(function () {
    if ($(this).val() == "YES") {
      $("#previousBank").empty();
      let input = ``;

      previousBank.map((item) => {
        for (let element in item) {
          if (element != "id") {
            if (Array.isArray(item[element])) {
              let Options = "";
              item[element].map(
                (item) =>
                  (Options += `<option value="${item}">${item}</option>`)
              );
              input += `
                  <select class="form-select" id=${item["id"]} name="${item["id"]}">
                    <option value="Please Select">Please Select</option>
                    ${Options}
                  </select>
                `;
            } else {
              input = `<input class="form-control" id="${item["id"]}" name="${item["id"]}" />`;
            }
            $("#previousBank").append(`
              <div class="row mt-1">
                <div class="col-sm-6 fw-bold d-flex gap-2">
                  <label class="form-label" for="bank name">${element}</label>
                  <p class="text-danger ">*</p>
                </div>
                <div class="col-sm-6">
                  ${input}
                </div>
              </div>
            `);
          }
        }
      });
    } else $("#previousBank").empty();
  });

  // form submission
  $("#serviceForm").submit(function (e) {
    e.preventDefault();
    const formData = new FormData(this);
    formSubmission.call(this, formData);
  });

  $("#PresentAddressContainer").on("blur", "input", function () {
    if ($("#SameAsPresent").prop("checked")) {
      copyPresentToPermanent();
    }
  });

  $("#ApplicantImage").change(function () {
    let file = this.files[0];
    if (!file) return;

    let reader = new FileReader();
    reader.onload = function (event) {
      $("#selectedImage").attr("src", event.target.result);
    };
    reader.readAsDataURL(file);
    $("#selectedImage").show();
  });

  //key press error check
  $("input").on("input", function (event) {
    const id = $(this).attr("id");

    // Check if the input is from a paste event
    const isPasteEvent =
      event.originalEvent &&
      event.originalEvent.inputType === "insertFromPaste";

    const isSuggestionEvent =
      event.originalEvent &&
      event.originalEvent.inputType === "insertReplacementText";

    if (isPasteEvent || isSuggestionEvent) {
      // If it's a paste event, capitalize the entire string
      const updatedValue = $(this).val().toUpperCase();
      $(this).val(updatedValue);
    } else {
      // If it's a keypress event, get the last character from the input
      const lastChar = $(this).val().slice(-1);

      // Check if the last character is a lowercase letter
      if (
        /[a-z]/.test(lastChar) &&
        id != "Email" &&
        id != "MobileNumber" &&
        id != "PresentPincode" &&
        id != "PermanentPincode" &&
        id != "AccountNumber"
      ) {
        // Convert the last character to uppercase
        const updatedValue =
          $(this).val().slice(0, -1) + lastChar.toUpperCase();
        $(this).val(updatedValue);
      }
    }

    if ($("#" + id + "Msg").length) {
      CheckErrors.call(this);
    }
  });

  $("#DateOfMarriage").change(function () {
    const dateOfMarriage = new Date($(this).val());

    var currentDate = new Date();

    var minDate = new Date(currentDate);
    minDate.setMonth(currentDate.getMonth() + 1);

    var maxDate = new Date(currentDate);
    maxDate.setMonth(currentDate.getMonth() + 6);

    if (dateOfMarriage >= minDate && dateOfMarriage <= maxDate) {
      setErrorSpan("DateOfMarriage", "");
    } else {
      setErrorSpan(
        "DateOfMarriage",
        "The range should be between 1 to 6 months from current date."
      );
    }
  });

  // blur error check
  $("input").blur(function () {
    CheckErrors.call(this);
  });

  $("select").change(function () {
    CheckErrors.call(this);
  });

  $("#pensionTypeContent").on("change", "select", function () {
    CheckErrors.call(this);
  });
});
