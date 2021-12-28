// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function deleteConfirmation(userId, isDeletedConfirm) {
    var deleteSpanId = 'deleteButtonSpan_' + userId;
    var confirmationSpanId = 'deleteConfirmationSpan_' + userId;
    if (isDeletedConfirm) {
        document.getElementById(deleteSpanId).style.display = "none";
        document.getElementById(confirmationSpanId).style.display = "inline";

    } else {
        document.getElementById(deleteSpanId).style.display = "inline";
        document.getElementById(confirmationSpanId).style.display = "none";
    }
}