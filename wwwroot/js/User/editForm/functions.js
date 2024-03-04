function calculateAge(dobInput) {
  const dob = new Date($("#" + dobInput).val());
  const currentDate = new Date();

  const timeDifference = currentDate - dob;

  const ageInYears = Math.floor(
    timeDifference / (365.25 * 24 * 60 * 60 * 1000)
  );

  $("#Age").val(ageInYears);
}

function updateDistrict(id, selectedValue) {
  const selectTehsil = determineSelectTehsil(id);

  emptyAndAddDefaultOption(selectTehsil);

  fetchAndPopulateOptions(
    "/User/GetTehsilsByDistrict",
    selectedValue,
    selectTehsil
  );

  if (id !== "District") {
    const selectedBlock = determineSelectedBlock(id);

    emptyAndAddDefaultOption(selectedBlock);

    fetchAndPopulateOptions(
      "/User/GetBlocksByDistrict",
      selectedValue,
      selectedBlock
    );
  }
}

function determineSelectTehsil(id) {
  return id === "District"
    ? "Tehsil"
    : id === "PresentDistrict"
    ? "PresentTehsil"
    : "PermanentTehsil";
}

function determineSelectedBlock(id) {
  return id === "PresentDistrict" ? "PresentBlock" : "PermanentBlock";
}

function emptyAndAddDefaultOption(selectId) {
  $("#" + selectId)
    .empty()
    .append(`<option value="Please Select">Please Select</option>`);
}

function fetchAndPopulateOptions(url, selectedValue, selectId) {
  $.ajax({
    url: url,
    method: "POST",
    data: { districtName: selectedValue },
    dataType: "json",
    success: function (data) {
      if (data.success) {
        const options = selectId.includes("Tehsil")
          ? data.tehsils
          : data.blocks;
        populateOptions(options, selectId);
      } else {
        console.error("Error: " + data.error);
      }
    },
    error: function (error) {
      console.error("AJAX Error: " + error);
    },
    complete: function () {
      selectCorrectOption(selectId);
    },
  });
}

function populateOptions(options, selectId) {
  if (options) {
    options.forEach((item) => {
      const name = selectId.includes("Tehsil")
        ? item.tehsilName
        : item.blockName;

      $("#" + selectId).append(
        `<option value="${item.uuid}">${name.toUpperCase()}</option>`
      );
    });
  }
}

function selectCorrectOption(selectId) {
  const id = makeFirstCharacterSmall(selectId);
  const value = citizenDetails[0][id].toUpperCase();
  $("#" + selectId + " option:contains('" + value + "')").prop(
    "selected",
    true
  );
}
