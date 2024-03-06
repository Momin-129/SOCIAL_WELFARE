const formFileds = JSON.parse(result.formElements);
const form = $("#serviceForm");

const citizenDocuments = JSON.parse(result.citizenDocuments);

$(document).ready(function () {
  $("#mainForm").on("click", "#docButton", function () {
    $("#serviceForm").hide();
    $("#docButton").hide();
    $("#submit").show();
    $("#serviceButton").show();
    $("#docForm").show();
  });

  $("#mainForm").on("click", "#serviceButton", function () {
    $("#serviceForm").show();
    $("#docButton").show();
    $("#submit").hide();
    $("#serviceButton").hide();
    $("#docForm").hide();
  });

  formFileds.forEach((item, index) => {
    const container = $("<div>").addClass(
      `container-fluid ${
        index > 0 && "border-top"
      } border-2 border-light d-flex flex-column justify-content-center align-items-center p-2 mt-4`
    );

    let row;
    if (item.section == "Present Address Details") {
      row = $("<div>")
        .addClass("row w-100")
        .attr({ id: "PresentAddressContainer" });
    } else {
      row = $("<div>").addClass("row w-100");
    }

    item.fields.forEach((field) => {
      let column = $("<div>").addClass(
        item.fields.length === 1 ? "col-sm-12" : "col-sm-6 mb-3 mt-3"
      );

      if (field.name == "SameAsPresent") {
        column = $("<div>").addClass("col-sm-12");
      }

      const label = $(`<label>`)
        .addClass("form-label text-white fw-bold")
        .attr({ for: field.name })
        .text(field.label);

      let input;
      if (field.type === "select") {
        input = $("<select>")
          .addClass(
            "form-select rounded-pill bg-transparent border border-light text-white"
          )
          .attr({
            name: field.name,
            id: field.name,
          });

        if (field.options) {
          field.options.forEach((option) =>
            input.append(`<option value="${option}">${option}</option>`)
          );
        }
      } else {
        input = $("<input>").attr({
          type: field.type,
          class:
            field.type != "checkbox" &&
            "form-control rounded-pill bg-transparent border border-light text-white",
          id: field.name,
          name: field.name,
          disabled: field.disabled,
        });
      }
      if (field.name == "ApplicantImage") {
        let img = $("<img>").attr({
          id: "selectedImage",
          src: "",
          style:
            "width:80px; height: 80px; display:none;border:2px solid white;border-radius:5px;margin-top:5px",
        });
        column.append(label, input, img);
      } else column.append(label, input);

      row.append(column);
    });

    container.append(row);
    form.append(container);
  });

  citizenDocuments.map((item) => {
    let options = ``;

    item.EnclosureDocument.map((option) => {
      options += `<option value="${option}">${option}</option>`;
    });
    const enclosureId = "enclosure" + item.label.split(" ").join("");
    const fileId = "file" + item.label.split(" ").join("");

    $("#documentContainer").append(`
        <tr>
          <th>${item.label}:</th>
          <td>
              <select name="${enclosureId}" class="form-select" id="${enclosureId}">
                  ${options}
              </select>
          </td>
          <td>
              <input type="file" name="${fileId}" id="${fileId}" class="form-control">
          </td>
        </tr>
    `);
  });

  $("#mainForm").append(`
        <div
          class="container-fluid border border-dark rounded-1 d-flex flex-column justify-content-center align-items-center p-2 mt-2">
          <div class="col-sm-12 text-white" style="height: 100px;">
              <p class="d-flex align-items-center justify-content-center py-1">
                  Department of Social Welfare, UT of J&K
              </p>
          </div>
          <div class="col-sm-12 d-flex justify-content-center mt-2 gap-2 p-1">
              <button type="button" class="btn text-white fs-6 rounded-pill px-4" style="background-color:#EF6603">Draft</button>
              <button id="docButton" type="button" class="btn text-white fs-6 rounded-pill px-4" style="background-color:#EF6603">Next</button>
              <button id="serviceButton" type="button" class="btn text-white fs-6 rounded-pill px-4" style="background-color:#EF6603">Previous</button>
              <button type="button" id="submit" class="btn text-white fs-6 rounded-pill px-4" style="background-color:#EF6603">Submit</button>
              <button type="submit" class="btn text-white fs-6 rounded-pill" id="finalSubmit"
        style="background-color: #EF6603;display:none">Submit</button>
              <button type="button" class="btn text-white fs-6 rounded-pill px-4" style="background-color:#EF6603">Close</button>
              <button type="reset" class="btn text-white fs-6 rounded-pill px-4" style="background-color:#EF6603">Reset</button>
          </div>
      </div>
`);

  $("#submit").hide();
  $("#serviceButton").hide();
});
