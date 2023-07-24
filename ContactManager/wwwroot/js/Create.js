// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

    console.log("Script loaded."); // Mensagem de log simples para verificar se o código está sendo executado

    // Função para preencher o select com os países e seus códigos usando a API REST Countries
    async function populateCountrySelect() {
            try {
                const response = await fetch("https://restcountries.com/v3.1/all");
    if (!response.ok) {
                    throw new Error("Failed to fetch countries");
                }
    const data = await response.json();
    var selectElement = document.querySelector(".country-select");

    // Limpar qualquer opção anterior no menu suspenso
    selectElement.innerHTML = "";

    // Adicionar a opção padrão com a mensagem informativa
    var defaultOption = document.createElement("option");
    defaultOption.value = "";
    defaultOption.textContent = "Select the country";
    selectElement.appendChild(defaultOption);

    // Preencher o menu suspenso com os países e seus códigos
    for (var i = 0; i < data.length; i++) {
                    var country = data[i];
    var countryCode = country.cca2 || country.ccn3 || country.cca3 || country.cioc;
    var countryName = country.name.common || country.name.official;
    var option = document.createElement("option");
    option.value = countryCode;
    option.textContent = countryName + " (" + countryCode + ")";
    selectElement.appendChild(option);
                }
            } catch (error) {
        console.error("Error fetching countries:", error);
            }
        }

    // Função para exibir o nome e código do país selecionado
    function showCountryInfo() {
            var selectedCountryOption = $(".country-select option:selected");
    var selectedCountryCode = selectedCountryOption.val();
    var selectedCountryName = selectedCountryOption.text().split(" (")[0];
            //if (selectedCountryCode && selectedCountryName) {
        //    alert("Selected Country: " + selectedCountryName + "\nCountry Code: " + selectedCountryCode);
        //}
    }

        // Chame a função para preencher o select quando a página estiver carregada
    document.addEventListener("DOMContentLoaded", function () {
        populateCountrySelect();
        });

    // Evento para exibir o nome e código do país selecionado quando houver uma seleção
    $(".country-select").change(function () {
        showCountryInfo();
        });

    // Código para adicionar outro campo de contato ao clicar no botão "Add Another Contact"
    $("#add-contact-btn").click(function () {
            var lastContactIndex = $(".contact").length - 1;
    var newContactIndex = lastContactIndex + 1;

    var newContactDiv = $("<div>", {class: "form-group contact" });
        newContactDiv.append($("<label>").text("Country Code"));
            var newSelect = $("<select>", {
                name: "Contacts[" + newContactIndex + "].CountryCode",
                class: "form-control country-select"
            });
                newContactDiv.append(newSelect);

                newContactDiv.append($("<span>", {class: "text-danger" }));
                    $("#contacts-container").append(newContactDiv);

                    populateCountrySelect();
        });
               

