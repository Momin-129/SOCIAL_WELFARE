const tableContainer = $("#applicationStatusModel");

// Create an array of promises
const fetchDataPromises = applications.map(async (element, index) => {
  const service = await fetchData(parseInt(element.serviceId));

  const row = `
  <tr style='cursor:pointer' onclick='window.location.href="/User/Documents?citizenId=${
    element.uuid
  }" '>
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
