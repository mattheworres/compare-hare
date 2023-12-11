import * as Yup from 'yup';

export const addManualFormValidationSchema = Yup.object({
  priceDate: Yup.date()
    .required('Price date is required'),
  price: Yup.number()
    .required('Price is required')
    .min(0.01, 'Price must be at least 1 cent USD')
});

export const addManualFormDefaultValues = {
  priceDate: null,
  price: null
};
