import * as Yup from 'yup';

export const paPower1stFormValidationSchema = Yup.object({
  name: Yup.string()
    .required('Alert name is required')
    .max(256, 'Name cannot be longer than 256 characters'),
  hasMinimumPrice: Yup.boolean(),
  minimumPrice: Yup.number()
    .when(['hasMaximumPrice', 'hasMinimumPrice'], {
      is: true,
      then: Yup.number().test('should-be-less-than-max', 'Cannot be greater than max', function (value) {
        const max = this.parent.maximumPrice;

        if (!max) return true;

        return value < max;
      }),
      otherwise: Yup.number().min(0, 'Minimum price must be positive')
    }),
  hasMaximumPrice: Yup.boolean(),
  maximumPrice: Yup.number()
    .when(['hasMaximumPrice', 'hasMinimumPrice'], {
      is: true,
      then: Yup.number().test('should-be-greater-than-min', 'Cannot be less than min', function (value) {
        const max = this.parent.minimumPrice;

        if (!max) return true;

        return value > max;
      }),
      otherwise: Yup.number().min(0, 'Maximum price must be positive')
    }),
  hasMinimumMonthLength: Yup.boolean(),
  minimumMonthLength: Yup.number()
    .integer('Minimum month length must be a whole number')
    .when(['hasMaximumMonthLength', 'hasMinimumMonthLength'], {
      is: true,
      then: Yup.number().test('should-be-less-than-max', 'Cannot be greater than max', function (value) {
        const max = this.parent.maximumMonthLength;

        if (!max) return true;

        return value < max;
      }),
      otherwise: Yup.number().min(0, 'Minimum month length must be positive')
    }),
  hasMaximumMonthLength: Yup.boolean(),
  maximumMonthLength: Yup.number()
    .integer('Maxmimum month length must be a whole number')
    .when(['hasMaximumMonthLength', 'hasMinimumMonthLength'], {
      is: true,
      then: Yup.number().test('should-be-greater-than-min', 'Cannot be less than min', function (value) {
        const min = this.parent.minimumMonthLength;

        if (!min) return true;

        return value > min;
      }),
      otherwise: Yup.number().min(0, 'Maximum month length must be positive')
    }),
});

export const paPower2ndFormValidationSchema = Yup.object({
  filterRenewable: Yup.boolean(),
  hasRenewable: Yup.boolean(),
  minimumRenewablePercent: Yup.number()
    .integer('Must be a whole number')
    .when(['filterRenewable', 'hasRenewable'], {
      is: true,
      then: Yup.number().test('should-be-less-than-max', 'Cannot be greater than max', function (value) {
        const min = this.parent.maximumRenewablePercent;

        if (!min) return true;

        return value < min;
      }),
      otherwise: Yup.number().min(0, 'Minimum renewable percent must be a positive number')
    }),
  maximumRenewablePercent: Yup.number()
    .integer('Must be a whole number')
    .max(100, 'Maximum renewable percent cannot be greater than 100')
    .when(['filterRenewable', 'hasRenewable'], {
      is: true,
      then: Yup.number().test('should-be-greater-than-min', 'Cannot be less than min', function (value) {
        const min = this.parent.minimumRenewablePercent;

        if (!min) return true;

        return value > min;
      }),
      otherwise: Yup.number().max(100, 'Maximum renewable percent can only be 100% or less')
    }),
  filterCancellationFee: Yup.boolean(),
  hasCancellationFee: Yup.boolean(),
  filterMonthlyFee: Yup.boolean(),
  hasMonthlyFee: Yup.boolean(),
  filterEnrollmentFee: Yup.boolean(),
  hasEnrollmentFee: Yup.boolean(),
  filterRequiresDeposit: Yup.boolean(),
  requiresDeposit: Yup.boolean(),
  filterBulkDiscounts: Yup.boolean(),
  hasBulkDiscounts: Yup.boolean(),
});

export const paPower3rdFormValidationSchema = Yup.object({
  comments: Yup.string().max(512, 'Comments cannot be longer than 512 characters'),
});

export const paPower1stFormDefaultValues = {
  name: '',
  hasMinimumPrice: false,
  minimumPrice: 0,
  hasMaximumPrice: false,
  maximumPrice: 0,
  hasMinimumMonthLength: false,
  minimumMonthLength: 0,
  hasMaximumMonthLength: false,
  maximumMonthLength: 0,
};

export const paPower2ndFormDefaultValues = {
  filterRenewable: false,
  hasRenewable: false,
  minimumRenewablePercent: 0,
  maximumRenewablePercent: 100,
  filterCancellationFee: false,
  hasCancellationFee: false,
  filterMonthlyFee: false,
  hasMonthlyFee: false,
  filterEnrollmentFee: false,
  hasEnrollmentFee: false,
  filterRequiresDeposit: false,
  requiresDeposit: false,
  filterBulkDiscounts: false,
  hasBulkDiscounts: false,
};

export const paPower3rdFormDefaultValues = {
  comments: '',
};
