$(document).ready(function () {
  const list = documentList.citizenDocuments;
  makeList(list);

  let formObject = JSON.parse(localStorage.getItem("formObject"));

  console.log(formObject);

  function processKey(key) {
    var words = key.split(/(?=[A-Z])|(?<=Id)(?=[A-Z])|(?<=[a-z])(?=[A-Z])/);

    for (var i = 0; i < words.length; i++) {
      if (words[i].toLowerCase() === "pre") {
        words[i] = "Present";
      } else if (words[i].toLowerCase() === "per") {
        words[i] = "Permanent";
      } else if (words[i].toLowerCase() == "acc") {
        words[i] = "Account";
      } else {
        words[i] =
          words[i].charAt(0).toUpperCase() + words[i].slice(1).toLowerCase();
      }

      if (words[i].toLowerCase() === "id") {
        words.splice(i, 1);
        i--;
      }
    }

    return words.join(" ");
  }

  var table = $("<table/>");
  table.addClass("table table-bordered border-light text-white rounded");
  var tbody = $("<tbody/>");

  // Create rows in the table
  for (var key in formObject) {
    if (formObject.hasOwnProperty(key)) {
      if (
        key.toLowerCase().endsWith("id") ||
        key.toLowerCase() === "sameaspresent" ||
        key.toLowerCase() === "agree" ||
        /^\d+$/.test(formObject[key])
      ) {
        continue; // Skip the rest of the loop and go to the next iteration
      }

      var label = processKey(key);
      var value = formObject[key];

      if (
        key == "districtName" ||
        key == "ApplicantName" ||
        key == "PresentAddress" ||
        key == "PermanentAddress" ||
        key == "BankName"
      ) {
        table.append(tbody);
        $("#formModalBody").append(table);
        var table = $("<table/>");
        table.addClass("table table-bordered border-light text-white mt-5");
        var tbody = $("<tbody/>");
      }

      var row = $("<tr/>");

      // Create cells for label and value
      var labelCell = $("<th/>");
      var valueCell = $("<td/>");

      labelCell.addClass("label-cell");

      // Set content for label and value cells
      labelCell.text(label);
      if (key.toLowerCase() === "applicantimage") {
        var img = $("<img/>");
        img.addClass("img-fluid");
        img.addClass("imgUser");

        img.attr({ src: localStorage.getItem(value) });

        valueCell.append(img);
      } else {
        valueCell.text(value);
      }
      row.append(labelCell);
      row.append(valueCell);
      tbody.append(row);
    }
  }
  table.append(tbody);

  $("#formModalBody").append(table);

  $("#documentForm").submit(function (e) {
    e.preventDefault();
    const formData = new FormData(this);
    let docs = JSON.parse(list);

    formData.append("docs", JSON.stringify(docs));

    const url = "/User/Documents";
    const options = {
      method: "POST",
      body: formData,
    };
    fetch(url, options)
      .then((response) => {
        if (!response.ok) {
          throw new Error("Network response was not ok");
        }
        return response.json();
      })
      .then((data) => {
        if (data.success) {
          const objectsList = data.objectsList;
          // Handle objectsList
          $("#modalButton").click();
          $("#modalButton").remove();
          var table = $("<table/>");
          table.addClass(
            "table table-bordered border-light text-white rounded"
          );
          var tbody = $("<tbody/>");
          objectsList.map((item) => {
            var absoluteUrl =
              applicationRoot + item.fileReference.replace("~", "");

            tbody.append(`
                <tr>
                    <th>${item.label}</th>
                    <td>
                        <a href="#" onclick="openInIframe('${absoluteUrl}'); return false;">
                    ${item.enclosure}
                </a>
                    </td>
                </tr>
            `);
          });
          table.append(tbody);
          $("#formModalBody").append(table);
          var scrollableDiv = $("#formModalBody");
          scrollableDiv.scrollTop(scrollableDiv.prop("scrollHeight"));
        } else {
          console.error("Server indicated failure");
        }
      })
      .catch((error) => {
        console.error("Error:", error);
      });
  });
});

function makeList(list) {
  const List = JSON.parse(list);
  List.map((item) => {
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
              <select name="${enclosureId}" class="form-select" id="${enclosureId}" required>
                  ${options}
              </select>
          </td>
          <td>
              <input type="file" name="${fileId}" id="${fileId}" class="form-control" required>
          </td>
        </tr>
    `);
  });
}

function openInIframe(url) {
  console.log(url);
  $("#myIframe").attr("src", url);

  $("#showDoc").click();
}
