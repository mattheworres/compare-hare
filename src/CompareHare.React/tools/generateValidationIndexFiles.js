import generateIndexFiles from './generateIndexFile';

generateIndexFiles({
  searchPath: '../src/features',
  includeTests: [
    /features(?:\/|\\).+?(?:\/|\\)validations/,
  ],
  formatInput: () => '*',
});
