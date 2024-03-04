let dynamicData = serviceList;
let itemsPerPage = 10;

function redirectToForm(data, id) {
  $.ajax({
    url: "/User/SetViewData", // Replace with the actual URL of your server-side action
    type: "POST",
    data: {
      serviceName: data,
      serviceId: id,
    },
    success: function () {
      window.location.href = "/User/Form";
    },
    error: function (xhr, status, error) {
      console.error("Error sending data:", error);
      console.log("Status:", status);
      console.log("Response:", xhr.responseText);
    },
  });
}
// Function to render a page of items
function renderPage(pageNumber) {
  const startIndex = (pageNumber - 1) * itemsPerPage;
  const endIndex = startIndex + itemsPerPage;
  const pageItems = dynamicData.slice(startIndex, endIndex);

  const listContainer = $("#dynamic-list");
  listContainer.empty();

  pageItems.forEach((item, index) => {
    const serviceId = item.serviceId;
    const actualIndex = startIndex + index + 1;
    const serviceName = item.serviceName;
    const listItem = `
        <tr>
            <td>${actualIndex}</td>
            <td><a class="text-decoration-none text-white" style="cursor:pointer;" onclick="redirectToForm('${serviceName}', '${serviceId}');" >${serviceName}</a></td>
            <td>${item.departmentName}</td>
            <td>${item.state}</td>
        </tr>
    `;
    listContainer.append(listItem);
  });
}

// Function to render pagination links
function renderPagination() {
  const totalPages = Math.ceil(dynamicData.length / itemsPerPage);
  const paginationContainer = $("#pagination");
  paginationContainer.empty();

  for (let i = 1; i <= totalPages; i++) {
    const pageLink = $("<li>")
      .addClass("page-item")
      .append(
        $("<a>")
          .addClass("page-link bg-dark text-white")
          .attr("href", "#")
          .text(i)
          .click(() => changePage(i))
      );
    paginationContainer.append(pageLink);
  }
}

// Function to handle page change
function changePage(pageNumber) {
  renderPage(pageNumber);
  highlightCurrentPage(pageNumber);
}

// Function to highlight the current page
function highlightCurrentPage(pageNumber) {
  $(".pagination .page-item").removeClass("active");
  $(".pagination .page-item:eq(" + (pageNumber - 1) + ")").addClass("active");
}

// Initial rendering
renderPage(1);
renderPagination();

// Function to filter data based on search term
function filterData(searchTerm) {
  return dynamicData.filter((item) =>
    item.serviceName.toLowerCase().includes(searchTerm.toLowerCase())
  );
}

// Function to handle search input changes
$("#searchInput").on("input keyup", function () {
  const searchTerm = $(this).val();
  if (searchTerm.trim() === "") {
    dynamicData = serviceList.slice(); // Restore to original data
  } else {
    const filteredData = filterData(searchTerm);
    dynamicData = filteredData;
  }
  renderPage(1); // Render the first page after filtering
  renderPagination();
});
