
function GiftViewModel(itemName, imageLocation, description, website, suggestedStores, isSpecificItemRequired, quantity, price) {
    this.itemName = ko.observable(itemName);
    this.imageLocation = ko.observable(imageLocation);
    this.description = ko.observable(description);
    this.website = ko.observable(website);
    this.suggestedStores = ko.observable(suggestedStores);
    this.specificItemRequried = ko.observable(isSpecificItemRequired);
    this.quantityRequired = ko.observable(quantity);
    this.retailPrice = ko.observable(price);


    this.requiredYesNo = ko.computed(function () {
        if (this.specificItemRequried()) {
            return "Yes";
        } else {
            return "No";
        }
    }, this);

    this.retailPriceText = ko.computed(function () {
        return "$" + this.retailPrice();
    }, this);

    this.suggestedStoresText = ko.computed(function () {
        if (this.suggestedStores()) {
            return this.suggestedStores();
        } else {
            return "N/A";
        }

    }, this);

}