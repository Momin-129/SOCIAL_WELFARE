function openInIframe(url) {
  $("#myIframe").attr("src", url);
  $("#showDoc").click();
}

function formatKey(key) {
  // Use regex to insert a space before every uppercase letter
  return (
    key
      .replace(/([A-Z])/g, " $1")
      // Capitalize the first letter of the resulting string
      .replace(/^./, (match) => match.toUpperCase())
  );
}

function getFirstKey(obj) {
  return Object.keys(obj)[0];
}

function getLastKey(obj) {
  return Object.keys(obj)[Object.keys(obj).length - 1];
}
