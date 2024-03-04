function processCitizenDetails(citizenDetails, mainContainer) {
  let table = $("<table/>");
  table.attr({
    class: "table table-bordered border-light text-white",
    style: 'table-layout:"fixed"',
  });

  let tbody = $("<tbody/>");

  for (let item in citizenDetails) {
    if (
      item !== "phases" &&
      item !== "remarks" &&
      item !== "formFields" &&
      !item.includes("id")
    ) {
      if (
        item == "presentAddress" ||
        item == "permanentAddress" ||
        item == "bankName" ||
        item == "documents"
      ) {
        mainContainer.append(table);
        table = $("<table/>");
        table.attr({
          class: "table table-bordered border-light text-white mt-5",
          style: 'table-layout:"fixed"',
        });

        tbody = $("<tbody/>");
      }
      if (item !== "formSpecific" && item !== "documents") {
        let td;
        if (item == "applicantImage") {
          var absoluteUrl =
            applicationRoot + citizenDetails[item].replace("~", "");
          td = `<img src="${absoluteUrl}" width=50 />`;
        } else {
          td = ` ${citizenDetails[item]}`;
        }
        const row = `
        <tr>
          <th>${formatKey(item)}</th>
          <td>${td}</td>
        </tr>
      `;
        tbody.append(row);
      } else if (item == "documents") {
        const documents = JSON.parse(citizenDetails[item]);
        documents.map((doc) => {
          var absoluteUrl =
            applicationRoot + doc.FileReference.replace("~", "");
          const row = `
           <tr>
            <th>${doc.label}</th>
            <td>
              <a href="#" onclick="openInIframe('${absoluteUrl}'); return false;">
                    ${doc.Enclosure}
                </a>
            </td>
           </tr>
        `;
          tbody.append(row);
        });
      } else if (item == "formSpecific") {
        const formSpecific = JSON.parse(citizenDetails[item]);
        for (let value in formSpecific) {
          if (!Number.isInteger(formSpecific[value])) {
            const row = `
              <tr>
                <th>${formatKey(value)}</th>
                <td>${formSpecific[value]}</td>
              </tr>
            `;
            tbody.append(row);
          }
        }
      }
      table.append(tbody);
    }
  }

  mainContainer.append(table);
}

let foundPending = false;
let currentPhase = "";
let nextPhase = "";

function processPhases(citizenDetails, returnObject) {
  const phases = JSON.parse(citizenDetails.phases);

  for (const key in phases) {
    if (foundPending) {
      nextPhase = key;
      break;
    }

    if (phases[key] === "Pending") {
      foundPending = true;
      currentPhase = key;
    }
  }

  $("#actionOptions").append(`
     <select class="form-select" name="action" id="action" required>
        <option value="Please Select">Please Select</option>
      ${nextPhase && `<option value="forward">Forward To ${nextPhase}</option>`}

        <option value="reject">Reject</option>
        ${
          currentPhase == getFirstKey(phases) &&
          `<option value="return">Return To Edit Application</option>`
        }
        ${
          currentPhase == getLastKey(phases) &&
          `<option value="sanction">Issue Sanction Letter</option>`
        }
    </select>
`);

  $("#action").change(function () {
    const action = $(this).val();
    if (action == "return") {
      $("#returnOptions").show();
      $("#tree").show();
      const ul = $("#tree").append("<ul></ul>");

      for (let item of returnObject) {
        let sublist = ``;
        item.fields.map((element) => {
          if (element.name !== "SameAsPresent") {
            sublist += `
         <li>
           <input type="checkbox" id="${element.name}" class="nested-checkbox form-check-input" value="${element.label}"> ${element.label}
         </li>`;
          }
        });
        $(ul).append(`
            <li>
              <label>
                  <input type="checkbox" class="mainCheck form-check-input"
                      value="${item.section}" /> ${item.section}
              </label>
              <ul>
                  ${sublist}
              </ul>
          </li>
        `);
      }
    } else {
      $("#tree").hide();
      $("#returnOptions").hide();
    }
  });

  $("#tree").on("change", ".mainCheck", function () {
    $(this)
      .parent()
      .parent()
      .find(".nested-checkbox")
      .prop("checked", $(this).prop("checked"));
  });

  $("#tree").hide();
}
