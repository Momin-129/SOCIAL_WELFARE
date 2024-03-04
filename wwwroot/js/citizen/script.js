const citizenDetails = result[0];
const mainContainer = $("#citizenDetails");
const returnObject = JSON.parse(citizenDetails.formFields);

console.log(citizenDetails);
// Citizen Details table
processCitizenDetails(citizenDetails, mainContainer);

// Action Phases
processPhases(citizenDetails, returnObject);

$("#actionForm").submit(function (e) {
  e.preventDefault();

  const checkedNestedCheckboxIds = $(this)
    .parent()
    .parent()
    .find(".nested-checkbox:checked")
    .map(function () {
      return this.id;
    })
    .get();

  returnObject.map((item) => {
    item.fields.map((field) => {
      if (checkedNestedCheckboxIds.includes(field.name)) {
        field.disabled = false;
      } else field.disabled = true;
    });
  });

  const formData = new FormData(this);
  formData.append("citizenId", citizenDetails.citizenId);
  formData.append("serviceId", citizenDetails.serviceId);
  formData.append("currentPhase", currentPhase);
  formData.append("nextPhase", nextPhase);
  formData.append("citizenDetails", JSON.stringify(citizenDetails));
  formData.append("returnObject", JSON.stringify(returnObject));

  let url = "";
  const action = $("#action").val();
  if (action == "forward") url = "/Citizen/ForwardAction";
  else if (action == "sanction") url = "/Citizen/SanctionAction";
  else if (action == "reject") url = "/Citizen/RejectAction";
  else if (action == "return") url = "/Citizen/ReturnAction";

  console.log(action);
  var requestOptions = {
    method: "POST",
    body: formData, // Use FormData as the body
  };

  // Fetch API
  fetch(url, requestOptions)
    .then((response) => response.json())
    .then((data) => {
      window.location.href = data.url;
    })
    .catch((error) => {
      console.error("Error:", error);
    });
});
