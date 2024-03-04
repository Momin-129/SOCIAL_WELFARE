// JavaScript to handle step navigation
$(document).ready(function () {
  let count = 0;
  const stepCount = $(".nav-pills li").length;

  function updateButtons() {
    $("#prevBtn").toggle(count !== 0);
    $("#nextBtn").toggle(count !== stepCount - 1);
  }

  $(".nav-pills a").click(function (e) {
    e.preventDefault();
    $(this).tab("show");
  });

  // Next button click handler
  $("#nextBtn").click(function (e) {
    e.preventDefault();
    var currentStep = $(".nav-pills li a.active").parent();
    var content = $(".content.active");

    currentStep.next().find("a.nav-link").removeClass("disabled").tab("show");
    currentStep.find("a.nav-link").addClass("disabled");

    content.next().addClass("active").show();
    content.removeClass("active").hide();

    count++;
    updateButtons();
  });

  // Previous button click handler
  $("#prevBtn").click(function (e) {
    e.preventDefault();
    var currentStep = $(".nav-pills li a.active").parent();
    var content = $(".content.active");

    currentStep.prev().find("a.nav-link").removeClass("disabled").tab("show");
    currentStep.find("a.nav-link").addClass("disabled");

    content.prev().addClass("active").show();
    content.removeClass("active").hide();

    count--;
    updateButtons();
  });

  updateButtons(); // Initial button state
});
