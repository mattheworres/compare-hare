import React from 'react';
import {Page} from '../../layout/components';
import {connect} from 'react-redux';
import {ProductsTable} from './index';
import {Grid} from '@material-ui/core';
import AddProduct1stModal from './create/AddProduct1stModal';
import AddProduct2ndModal from './create/AddProduct2ndModal';
import AddProduct3rdModal from './create/AddProduct3rdModal';
import AddProduct4thModal from './create/AddProduct4thModal';
class ProductsDashboard extends React.PureComponent {
  render() {
    return (
      <Page>
        <Grid container spacing={16}>
          <Grid item xs={false} sm={false} md={false} lg={1} />
          <Grid item xs={12} sm={12} md={12} lg={10}>
            <ProductsTable />
          </Grid>
          <Grid item xs={false} sm={false} md={false} lg={1} />
        </Grid>
        <AddProduct1stModal />
        <AddProduct2ndModal />
        <AddProduct3rdModal />
        <AddProduct4thModal />
      </Page>
    )
  }
}

function mapStateToProps() {
  return {

  };
}

const mapDispatchToProps = {};

export default connect(mapStateToProps, mapDispatchToProps)(ProductsDashboard);
