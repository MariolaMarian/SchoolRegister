$("#subjects-filter-input").change(function () {
    const filterValue = $("#subjects-filter-input").val();
    console.log(filterValue);
    $.get("/Subject/Index", $.param({ filterValue: filterValue }), function (result) {
        $(".subjects-data-table").html(result);
    })
})

$("#students-filter-input").change(function () {
    const filterValue = $("#students-filter-input").val();
    console.log(filterValue);
    $.get("/Student/Index", $.param({ filterValue: filterValue }), function (result) {
        $(".students-data-table").html(result);
    })
})