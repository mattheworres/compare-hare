import * as Yup from 'yup';

export const addProduct1stFormValidationSchema = Yup.object({
  name: Yup.string()
    .required('Product name is required')
    .min(4, 'Name must be at least 4 characters long')
    .max(512, 'Name cannot be longer than 512 characters'),
});

export const addProduct1stFormDefaultValues = {
  name: '',
};

export const addProduct2ndFormValidationSchema = Yup.object({
  productRetailer: Yup.number()
    .integer('Product retailer must be a whole number (why are you seeing this??)')
    .required('You must pick a retailer from the list first'),
  otherRetailerDisplayName: Yup.string()
    .when(['productRetailer'], {
      is: productRetailer => {
        return parseInt(productRetailer, 10) === 1001
      }, // TODO: magic number
      then: Yup.string()
        .required('You must give this retailer a custom display name')
        .min(4, 'Retailer name needs to be at least 4 letters long')
        .max(512, 'Retailer name can\'t have a name longer than 512 characters')
    })
});

export const addProduct2ndFormDefaultValues = {
  productRetailer: 0,
  otherRetailerDisplayName: ''
};

export const addProduct3rdFormValidationSchema = Yup.object({
  scrapeUrl: Yup.string()
    .required('You must provide a URL')
    .url('You must provide a valid URL')
    .max(512, 'URL cannot be longer than 512 characters'),
  priceSelector: Yup.string()
    .optional()
    .min(2, 'Selector must be at least 2 characters long')
    .max(128, 'Selector cannot be longer than 128 characters')
})

export const addProduct3rdFormDefaultValues = {
  scrapeUrl: '',
  priceSelector: ''
};
