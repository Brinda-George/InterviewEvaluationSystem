$(document).ready(function () {
    $(function () {
        $('.edit-mode').hide();
        $('.edit-user, .cancel-user').on('click', function () {
            var tr = $(this).parents('tr:first');
            tr.find('.edit-mode, .display-mode').toggle();
        });

        $('.save-user').on('click', function () {
            var tr = $(this).parents('tr:first');
            var Name = tr.find("#Name").val();
            var Email = tr.find("#Email").val();
            var Designation = tr.find("#Designation").val();
            var EmployeeId = tr.find("#lblEmployeeId").html();
            $.ajax({
                url: '/HR/UpdateInterviewer/',
                data: JSON.stringify({ "EmployeeId": EmployeeId, "Name": Name, "Email": Email, "Designation": Designation }),
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                success: function (data) {

                    tr.find('.edit-mode, .display-mode').toggle();
                    tr.find("#lblName").text(data.Name);
                    tr.find("#lblDesignation").text(data.Designation);
                    tr.find("#lblEmail").text(data.Email);
                }
            });

        });

        $('.delete-user').on('click', function () {
            var tr = $(this).parents('tr:first');
            var EmployeeId = tr.find("#lblEmployeeId").html();
            $.ajax({
                url: '/HR/DeleteInterviewer/',
                data: JSON.stringify({ "EmployeeId": EmployeeId }),
                type: 'POST',
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    alert('Successfully Deleted Interviewer');
                }
            })
        });

    });
});
