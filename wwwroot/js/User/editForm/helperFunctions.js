function createContainer(item, index) {
  return $("<div>").addClass(
    `container-fluid ${
      index > 0 && "border-top"
    } border-2 border-light d-flex flex-column justify-content-center align-items-center p-2 mt-4`
  );
}

function createRow(item) {
  const row = $("<div>")
    .addClass(
      item.section == "Present Address Details" ? "row w-100" : "row w-100"
    )
    .attr({
      id:
        item.section == "Present Address Details"
          ? "PresentAddressContainer"
          : undefined,
    });
  return row;
}

function getFieldValue(field) {
  const id = makeFirstCharacterSmall(field.name);
  if (field.name == "DOB") {
    return citizenDetails[0]["dob"].split("T")[0];
  } else if (field.isFormSpecific) {
    const formSpecific = JSON.parse(citizenDetails[0].formSpecific);
    return formSpecific[field.name];
  } else {
    return citizenDetails[0][id];
  }
}

function createColumn(item, field) {
  const column = $("<div>").addClass(
    item.fields.length === 1 ? "col-sm-12" : "col-sm-6 mb-3 mt-3"
  );
  return column;
}

function createLabel(field) {
  return $(`<label>`)
    .addClass("form-label text-white fw-bold")
    .attr({ for: field.name })
    .text(field.label);
}

function createInput(field, value) {
  if (field.type === "select") {
    const input = $("<select>").addClass("form-select rounded-pill").attr({
      name: field.name,
      id: field.name,
      disabled: field.disabled,
    });

    if (field.options) {
      field.options.forEach((option) =>
        input.append(
          `<option value="${option}">${option.toUpperCase()}</option>`
        )
      );
    }

    return input;
  } else {
    return $("<input>").attr({
      type: field.type,
      class: field.type != "checkbox" && "form-control rounded-pill",
      id: field.name,
      name: field.name,
      disabled: field.disabled,
      value: value,
    });
  }
}

function createImage(value) {
  const absoluteUrl = applicationRoot + value.replace("~", "");
  return $("<img>").attr({
    id: "selectedImage",
    src: absoluteUrl,
    style:
      "width:80px; height: 80px;border:2px solid white;border-radius:5px;margin-top:5px",
  });
}
