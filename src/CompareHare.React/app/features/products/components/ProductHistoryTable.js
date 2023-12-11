import React from 'react';
import {connect} from 'react-redux';
import autobind from 'class-autobind';
import PropTypes from 'prop-types';
import {
  withStyles,
  Paper,
  Typography
} from '@material-ui/core';
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  Title,
  Tooltip,
  Legend,
} from 'chart.js';
import { Line } from 'react-chartjs-2';

ChartJS.register(
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  Title,
  Tooltip,
  Legend
);

const styles = theme => ({
  root: {
    ...theme.mixins.gutters(),
    width: '100%',
    marginTop: theme.spacing.unit * 3,
    overflowX: 'auto'
  },
  title: {
    margin: '20px'
  },
  product: {
    ...theme.mixins.gutters()
  },
  positive: {
    color: '#4caf50',
    verticalAlign: 'bottom'
  },
  negative: {
    color: '#f44336',
    verticalAlign: 'bottom'
  },
  emptyRow: {
    textAlign: 'center'
  }
});

const options = {
  responsive: true,
  plugins: {
    legend: {
      position: 'top',
    },
    title: {
      display: true,
      text: 'Product ABCDA Price History',
    },
  },
  scales: {
    xAxes: [{
      title: "time",
      type: 'time',
      gridLines: {
        lineWidth: 2
      },
      time: {
        unit: "day",
        unitStepSize: 1000,
        parser: 'MMM D YYYY',
        displayFormats: {
          millisecond: 'MMM D YYYY',
          second: 'MMM D YYYY',
          minute: 'MMM D YYYY',
          hour: 'MMM D YYYY',
          day: 'MMM D YYYY',
          week: 'MMM D YYYY',
          month: 'MMM D YYYY',
          quarter: 'MMM D YYYY',
          year: 'MMM D YYYY',
        }
      }
    }]
  }
};

const labels1 = ['Jan 5 2022', 'Jan 22 2022', 'Feb 2 2022', 'Mar 23 2022'];
const labels2 = ['Jan 22 2022', 'Mar 7 2022', 'Mar 23 2022', 'Apr 9 2022'];

const labels = labels1.concat(labels2);

labels.sort((a, b) => {
  const dateA = new Date(a),
    dateB = new Date(b);

  return dateA - dateB;
});

const data = {
  labels,
  datasets: [
    {
      label: 'Lowes',
      data: labels1.map((label, idx) => idx === 2 ? null : 1558.59 + idx),
      borderColor: 'rgb(255, 99, 132)',
      backgroundColor: 'rgba(255, 99, 132, 0.5)',
    },
    {
      label: 'Home Depot',
      data: labels.map((label, idx) => 1557.99 + idx),
      borderColor: 'rgb(53, 162, 235)',
      backgroundColor: 'rgba(53, 162, 235, 0.5)',
    },
  ],
};

class ProductHistoryTable extends React.PureComponent {
  constructor(props) {
    super(props);

    autobind(this);

    this.state = {
      productId: null,
      menuAnchor: null,
      menuRetailerId: null,
      menuRetailerName: null,
      menuRetailerEnabled: false
    };
  }

  render() {
    const { classes } = this.props;
    console.log('shmoyoho', labels.join(','));
    return (
      <Paper className={classes.root}>
        <Line options={options} data={data} />;
      </Paper>
    );
  }
}

ProductHistoryTable.propTypes = {
  classes: PropTypes.object,
  loadProductCurrent: PropTypes.func.isRequired,
  trackedProductId: PropTypes.string,
};

function mapStateToProps() {
  return {
    
  };
}

const mapDispatchToProps = {
  
};

export default connect(mapStateToProps, mapDispatchToProps)(withStyles(styles)(ProductHistoryTable));