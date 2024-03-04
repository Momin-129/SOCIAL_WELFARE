$(document).ready(function () {
  const listContainer = $("#dynamic-list");
  listContainer.empty();

  if (citizenList.length == 0) {
    listContainer.append(
      `<tr>
        <td colspan=3>
          <p class="text-center fs-3 fw-bold w-100 border border-dark">NO RECORDS</p>
        </td>
      </tr>
      `
    );
  } else {
    citizenList.forEach((item, index) => {
      const listItem = `
        <tr class="text-center" style="cursor:pointer;"  onclick="citizenDatail(${
          item.uuid
        });">
            <td>${index + 1}</td>
            <td><a class="text-decoration-none text-white" >${
              item.uuid
            }</a></td>
            <td>${item.applicantName}</td>
        </tr>
    `;
      listContainer.append(listItem);
    });
  }
});

function citizenDatail(id) {
  window.location.href = "/Citizen/CitizenDetail?id=" + id;
}
