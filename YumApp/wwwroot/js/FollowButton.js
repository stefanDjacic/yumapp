$(document).ready(function () {
    $('#followButton').click(function () {

        var currentUserId = @currentLoggedInUserId;

        $.post("/User/Follow/")
    });
});