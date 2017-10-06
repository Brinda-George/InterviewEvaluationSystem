
    $(function () {  
        $('.edit-mode').hide();  
        $('.edit-user, .cancel-user').on('click', function () {  
            var tr = $(this).parents('tr:first');  
            tr.find('.edit-mode, .display-mode').toggle();  
        });  
   
        $('.save-user').on('click', function () {  
            var tr = $(this).parents('tr:first');  
            var Name = tr.find("#Name").val();  
            var SurName = tr.find("#SurName").val();  
            var UserID = tr.find("#UserID").html();  
            tr.find("#lblName").text(Name);  
            tr.find("#lblSurName").text(SurName);  
            tr.find('.edit-mode, .display-mode').toggle();  
            var UserModel =  
            {  
                "ID": UserID,  
                "Name": Name,  
                "SurName": SurName  
            };  
            $.ajax({  
                url: '/User/ChangeUser/',  
                data: JSON.stringify(UserModel),  
                type: 'POST',  
                contentType: 'application/json; charset=utf-8',  
                success: function (data) {  
                    alert(data);  
                }  
            });  
   
        });  
    })  
