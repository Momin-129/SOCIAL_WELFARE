const formFields = JSON.parse(citizenDetails[0].formFields);
const form = $("#serviceForm");


$("#citizenId").val(citizenDetails[0]["citizenId"]);
$("#presentAddressId").val(citizenDetails[0]["presentAddressId"]);
$("#permanentAddressId").val(citizenDetails[0]["permanentAddressId"]);
$("#bankDetailsId").val(citizenDetails[0]["bankDetailsId"]);
// Since database returned column make first character small
function makeFirstCharacterSmall(inputString) {
  return inputString.charAt(0).toLowerCase() + inputString.substring(1);
}

// to update select tags after they are dynamically populated
function setSelectTags() {
  formFields.forEach((item) => {
    item.fields.forEach((field) => {
      if (field.name == "DOB") {
        calculateAge("DOB");
      }

      if (field.isFormSpecific && field.type == "select") {
        const formSpecific = JSON.parse(citizenDetails[0].formSpecific);
        let value = formSpecific[field.name];
        $("#" + field.name).val(value);
      }

      if (field.type == "select" && field.name != "District") {
        let id = makeFirstCharacterSmall(field.name);
        let value = citizenDetails[0][id].toUpperCase();
        $("#" + field.name + " option:contains('" + value + "')").prop(
          "selected",
          true
        );
        if (field.name.includes("District")) {
          updateDistrict(field.name, value);
        }
      }
    });
  });
}

// to dynamically append district values
function appendDistrictOptions(districtList, elementId) {
  const element = $("#" + elementId);
  element.append(`<option value="Please Select">Please Select</option>`);

  districtList.forEach((item) => {
    element.append(
      `<option value="${item.uuid}" id="${item.districtName}">
        ${item.districtName.toUpperCase()}
      </option>`
    );
  });
}

$(document).ready(function () {
  // AJAX call for getting districts
  $.ajax({
    url: "/User/GetDistricts",
    method: "GET",
    dataType: "json",
    success: function (data) {
      if (data.success) {
        appendDistrictOptions(data.districts, "District");
        appendDistrictOptions(data.districts, "PresentDistrict");
        appendDistrictOptions(data.districts, "PermanentDistrict");
      } else {
        console.error("Error: " + data.error);
      }
    },
    error: function (error) {
      console.error("AJAX Error: " + error);
    },
    complete: function () {
      setSelectTags();
    },
  });

  // generate the form dynamically
  formFields.forEach((item, index) => {
    const container = createContainer(item, index);

    const row = createRow(item);

    item.fields.forEach((field) => {
      const value = getFieldValue(field);

      if (field.name == "SameAsPresent") {
        return;
      }

      const column = createColumn(item, field);

      const label = createLabel(field);
      const input = createInput(field, value);

      if (field.name == "ApplicantImage") {
        const img = createImage(value);
        column.append(label, input, img);
      } else {
        column.append(label, input);
      }

      row.append(column);
    });

    container.append(row);
    form.append(container);
  });

  form.append(`
  <div
    class="container-fluid border border-dark rounded-1 d-flex flex-column justify-content-center align-items-center p-2 mt-2">
    <div class="col-sm-12 text-white" style="height: 100px;">
        <p class="d-flex align-items-center justify-content-center py-1">
            Department of Social Welfare, UT of J&K
        </p>
    </div>
    <div class="col-sm-12 d-flex justify-content-center mt-2 gap-2 p-1">
        <button type="button" class="btn text-white border border-2 rounded-pill px-4">Draft</button>
        <button type="submit" class="btn text-white border border-2 rounded-pill px-4">Submit</button>
        <button type="button" class="btn text-white border border-2 rounded-pill px-4">Close</button>
        <button type="reset" class="btn text-white border border-2 rounded-pill px-4">Reset</button>
    </div>
</div>
`);

  // to display the image base on the file choosen.
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

  // form submission
  $("#serviceForm").submit(function (e) {
    e.preventDefault();
    const formData = new FormData(this);

    for (const [fieldName, fieldValue] of formData.entries()) {
      const element = formFields
        .flatMap((section) => section.fields)
        .find((field) => field.name === fieldName);

      console.log(element);
      if (element !== undefined && element.isFormSpecific) {
        formData.delete(fieldName);

        const formSpecific = JSON.parse(citizenDetails[0]["formSpecific"]);
        formSpecific[fieldName] = fieldValue;

        formData.set("FormSpecific", JSON.stringify(formSpecific));
      }
    }

    fetch("/User/EditForm", {
      method: "POST",
      body: formData,
    })
      .then((response) => response.json())
      .then((data) => {
        if (data.success) {
          window.location.href = data.url;
        }
      })
      .catch((error) => console.error("Error:", error));
  });
});
