$(document).ready(function () {
    $(".articleName").AssignButtonClicked(function () {
        var itemData = $(this).data('data-assigned-id');

            $.ajax({
                url: '/myprtctd/ShowArticle', type: "POST", data: itemData
                , success: function () { alert("OK") }, error: function () { alert("problem calling ajax") }
            });
    });
            $('#addButton').click(function () {
            alert("Hello Man");
    });
});


function myFunction() {
    $.ajax({
        url: '/myprtctd/ShowArticle',
        data: { id: id },
        success: function () {
            alert('Added');
        }
    });
}
