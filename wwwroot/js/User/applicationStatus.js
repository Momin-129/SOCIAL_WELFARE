const tableContainer = $("#applicationStatusModel");

let citizenId;

function IndividualApplication(Phases, Remarks) {
  const modal = $("#individualStatus");
  modal.empty();
  for (const item in Phases) {
    if (Phases[item] == "Returned") {
      $("#editButton").remove();
      modal.parent().after(`
  <div class='d-flex justify-content-center'>
    <button class='btn btn-primary' id="editButton" onclick='editForm("${item}")'>Edit Form</button>
  </div>
`);
    } else if (Phases[item] == "Pending" || Phases == "Reject") {
      modal.append(`
          <tr>
            <td>${item}</td>
            <td>${Phases[item]}</td>
            <td>${Remarks[item]}</td>
          </tr>
    `);
      break;
    }
    modal.append(`
          <tr>
            <td>${item}</td>
            <td>${Phases[item]}</td>
            <td>${Remarks[item]}</td>
          </tr>
    `);
  }

  $("#statusButton").click();
}

// Create an array of promises
const fetchDataPromises = applications.map(async (element, index) => {
  const service = await fetchData(parseInt(element.serviceId));
  const result = await fetchApplicantDetails(parseInt(element.uuid));
  citizenId = result.citizenId;
  const row = `
  <tr style='cursor:pointer' onclick='IndividualApplication(${result.phases},${
    result.remarks
  });'>
    <td>${index + 1}</td>
    <td>${service.serviceName}</td>
    <td>${element.dateOfSubmission.split("T")[0]}</td>
  </tr>
`;
  return row;
});

// Wait for all promises to resolve
Promise.all(fetchDataPromises)
  .then((rows) => {
    // Now, all the fetchData calls are completed, and rows are in order
    tableContainer.append(rows.join(""));
  })
  .catch((error) => {
    console.error("Error fetching data:", error);
  });

async function fetchData(id) {
  const response = await fetch(`/User/GetServiceName?Id=${id}`);
  const data = await response.json();
  return data.service;
}

async function fetchApplicantDetails(citizenId) {
  const response = await fetch(
    `/User/GetApplicationDetails?citizenId=${citizenId}`,
    {
      method: "GET",
      headers: {
        "Content-Type": "application/json",
      },
    }
  );
  const result = await response.json();
  return result.result[0];
}

async function editForm(phase) {
  window.location.href =
    "/User/EditForm?citizenId=" + citizenId + "&phase=" + phase;
}
