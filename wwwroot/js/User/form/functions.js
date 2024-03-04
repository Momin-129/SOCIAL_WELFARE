function setErrorSpan(id, msg) {
  if ($("#" + id + "Msg").length) {
    $("#" + id + "Msg").text(msg);
  } else {
    const newSpan = $("<span>")
      .attr({ id: id + "Msg", class: "errorMsg" })
      .text(msg);
    $("#" + id).after(newSpan);
  }
}

function CheckErrors() {
  const id = $(this).attr("id");
  if ($("#" + id).is("select")) {
    if ($(this).val() == "Please Select") {
      setErrorSpan(id, "This field is required.");
    } else setErrorSpan(id, "");
  } else if ($(this).val().length == 0 && id != "ApplicantImage") {
    setErrorSpan(id, "This field is required");
  } else if (id == "name" || id == "guardian") {
    if (!/^[A-Za-z .']+$/.test($(this).val())) {
      setErrorSpan(
        id,
        "Please use letters (a-z, A-Z) and special characters (. and ') only."
      );
    } else setErrorSpan(id, "");
  } else if (
    id == "MobileNumber" ||
    id == "PresentPincode" ||
    id == "PermanentPincode" ||
    id == "AccountNumber"
  ) {
    let maxLenght = 0;
    if (id == "MobileNumber") maxLenght = 10;
    else if (id == "PresentPincode" || id == "PermanentPincode") maxLenght = 6;
    else if (id == "AccountNumber") maxLenght = 16;

    if (!/^\d+$/.test($(this).val())) {
      setErrorSpan(id, "Please enter only digits");
    } else if ($(this).val().length < maxLenght) {
      setErrorSpan(id, `Please enter at least ${maxLenght} characters.`);
    } else {
      setErrorSpan(id, "");
    }
  } else if (
    id !== "PresentAddress" &&
    id !== "PermanentAddress" &&
    id != "DOB" &&
    id != "ApplicantImage" &&
    id != "Email" &&
    id != "IfcsCode" &&
    id != "agree" &&
    id != "SameAsPresent" &&
    id != "DateOfMarriage" &&
    id != "IfscCode"
  ) {
    if (!/^[A-Za-z\s]+$/.test($(this).val())) {
      setErrorSpan(id, "Please enter only charater(s).");
    } else if ($(this).val().length < 3) {
      setErrorSpan(id, "Please enter at least 3 character(s).");
    } else setErrorSpan(id, "");
  } else setErrorSpan(id, "");
}

function updateDistrict(id, selectedValue) {
  const selectTehsil =
    id == "District"
      ? "#Tehsil"
      : id == "PresentDistrict"
      ? "#PresentTehsil"
      : "#PermanentTehsil";

  $(selectTehsil).empty();
  $(selectTehsil).append(
    `<option value="Please Select">Please Select</option>`
  );

  $.ajax({
    url: "/User/GetTehsilsByDistrict",
    method: "POST",
    data: { districtName: selectedValue },
    dataType: "json",
    success: function (data) {
      if (data.success) {
        // Access the list of Tehsils
        var tehsils = data.tehsils;
        selectedValue !== "Please Select" &&
          tehsils.map((item) =>
            $(selectTehsil).append(
              `<option value="${
                item.uuid
              }">${item.tehsilName.toUpperCase()}</option>`
            )
          );
      } else {
        console.error("Error: " + data.error);
      }
    },
    error: function (error) {
      console.error("AJAX Error: " + error);
    },
  });

  if (id != "District") {
    const selectedBlock =
      id == "PresentDistrict" ? "#PresentBlock" : "#PermanentBlock";

    $(selectedBlock).empty();
    $(selectedBlock).append(
      `<option value="Please Select">Please Select</option>`
    );

    $.ajax({
      url: "/User/GetBlocksByDistrict",
      method: "POST",
      data: { districtName: selectedValue },
      dataType: "json",
      success: function (data) {
        if (data.success) {
          var blocks = data.blocks;
          selectedValue !== "Please Select" &&
            blocks.map((item) =>
              $(selectedBlock).append(
                `<option value="${
                  item.uuid
                }">${item.blockName.toUpperCase()}</option>`
              )
            );
        } else {
          console.error("Error: " + data.error);
        }
      },
      error: function (error) {
        console.error("AJAX Error: " + error);
      },
    });
  }
}

function updatePensionType(selectedValue) {
  $("#pensionTypeContent").empty();
  if (PensionType.hasOwnProperty(selectedValue)) {
    const type = PensionType[selectedValue];

    type.map((item) => {
      for (value in item) {
        const col = $(`<div class="col-sm-12 mb-1 mt-1"></div>`);
        if (value != "id") {
          $(col).append(`<label class="form-label">${value}</label>`);
          if (Array.isArray(item[value])) {
            let Options = "";
            item[value].map(
              (a) => (Options += ` <option value=${a}>${a}</option>`)
            );
            $(col).append(`
               
                  <select class="form-select rounded border-dark" name=${item["id"]} id=${item["id"]} required>
                    <option value="Please Select">
                      Please Select
                    </option>
                    ${Options}
                  </select>
                </div>
              `);
          } else {
            $(col).append(`
                
                  <input type="text" class="form-control rounded border-dark" name=${item["id"]} id=${item["id"]} />
                
              `);
          }
        }
        $("#pensionTypeContent").append(
          `<div class="row w-100 ">${col[0].outerHTML}</div>`
        );
      }
    });
  }
}

function copyPresentToPermanent() {
  $("#PermanentAddress").val($("#PresentAddress").val());
  $("#PermanentDistrict").val($("#PresentDistrict").val());
  $("#PermanentDistrict").trigger("change"); // to trigger and populate the tehsil select tag
  setTimeout(() => {
    $("#PermanentTehsil").val($("#PresentTehsil").val());
    $("#PermanentBlock").val($("#PresentBlock").val());
  }, 500);
  $("#PermanentPanchayatMuncipality").val(
    $("#PresentPanchayatMuncipality").val()
  );
  $("#PermanentVillage").val($("#PresentVillage").val());
  $("#PermanentWard").val($("#PresentWard").val());
  $("#PermanentPincode").val($("#PresentPincode").val());
}

function empytPermanent() {
  $("#PermanentAddress").val("");
  $("#PermanentDistrict").val("Please Select");
  $("#PermanentBlock").val("");
  $("#PermanentPanchayatMuncipality").val("");
  $("#PermanentVillage").val("");
  $("#PermanentWard").val("");
  $("#PermanentPincode").val("");
  $("#PermanentDistrict").trigger("change"); // to trigger and empty the tehsil select tag
}

function formSubmission(formData) {
  console.log($(this).attr("id"));
  const formObject = {};

  formData.forEach(function (value, key) {
    if (value instanceof File && value.size === 0) {
      formObject[key] = "";
    } else if (value instanceof File && value.size != 0) {
      var reader = new FileReader();
      reader.onload = function (e) {
        localStorage.setItem(key + "Data", e.target.result);
      };
      reader.readAsDataURL(value);
      formObject[key] = key + "Data";
    } else {
      formObject[key] = value;
      if (
        key.toLocaleLowerCase().includes("district") ||
        key.toLocaleLowerCase().includes("tehsil") ||
        key.toLocaleLowerCase().includes("block")
      ) {
        formObject[key + "Name"] = $(
          "#" + key + " option[value='" + value + "']"
        ).text();
      }
    }
  });

  let hasErrors = false;
  console.log(formObject);
  for (let key in formObject) {
    if (formObject[key] == "" || formObject[key] == "Please Select") {
      setErrorSpan(key, "This field is required");
      hasErrors = true;
    }
  }

  if (!hasErrors) {
    localStorage.setItem("formObject", JSON.stringify(formObject));
    this.submit();
  }
}