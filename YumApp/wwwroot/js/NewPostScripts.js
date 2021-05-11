//Start of ajax call for ingredients
    $(document).ready(function () {
        $('#add-new-post-btn').click(function () {
            $.ajax({
                type: 'GET',
                url: '/User/GetIngredients',
                success: function (ingredients) {
                    var ingredientDiv = document.getElementById('add-new-post-ingredients');
                    ingredientDiv.innerHTML = '';

                    ingredients.forEach((element) => {
                        var formCheckDiv = document.createElement('div');
                        formCheckDiv.classList.add('form-check');

                        var ingredientHtml = `
                                    <input class="form-check-input" type="checkbox" value="${element.id}" id="ingredient-${element.id}">
                                    <label class="form-check-label" for="ingredient-${element.id}">
                                        ${element.name}
                                    </label>
                                `;

                        formCheckDiv.innerHTML += ingredientHtml;

                        ingredientDiv.appendChild(formCheckDiv);
                    });
                },
                error: function (response) {
                    alert(response.responseText);
                }
            });
        });
    });
//End of ajax call for ingredients

//Start of ajax call for posting new post
    $(document).ready(function () {
        $('#post-new-post-btn').click(function () {
            var content = $('#add-new-post-textarea-content').val();
            console.log(content);
            var notes = $('#add-new-post-textarea-notes').val();
            console.log(notes);
            var ingredientIds = [];
            $('#add-new-post-ingredients input:checked').each(function () {
                ingredientIds.push($(this).attr('value'));
            });
            console.log(ingredientIds);

            $.ajax({
                type: 'POST',
                url: '/User/PostAPost',
                contentType: 'application/x-www-form-urlencoded',
                data: { postContent: content, postNotes: notes, postIngredientIds: ingredientIds },
                success: function (response) {
                    window.location.href = response.redirectToUrl;
                },
                error: function (response) {
                    alert(response.responseText);
                }
            });
        });
    });
//End of ajax call for posting new post