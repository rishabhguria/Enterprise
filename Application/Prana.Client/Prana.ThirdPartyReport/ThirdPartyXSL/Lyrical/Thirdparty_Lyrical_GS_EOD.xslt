<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>


	<xsl:template name="MonthName">
		<xsl:param name="Month"/>
		<xsl:choose>
			<xsl:when test="$Month=1">
				<xsl:value-of select="'JAN'"/>
			</xsl:when>
			<xsl:when test="$Month=2">
				<xsl:value-of select="'FEB'"/>
			</xsl:when>
			<xsl:when test="$Month=3">
				<xsl:value-of select="'MAR'"/>
			</xsl:when>
			<xsl:when test="$Month=4">
				<xsl:value-of select="'APR'"/>
			</xsl:when>
			<xsl:when test="$Month=5">
				<xsl:value-of select="'MAY'"/>
			</xsl:when>
			<xsl:when test="$Month=6">
				<xsl:value-of select="'JUN'"/>
			</xsl:when>
			<xsl:when test="$Month=7">
				<xsl:value-of select="'JUL'"/>
			</xsl:when>
			<xsl:when test="$Month=8">
				<xsl:value-of select="'AUG'"/>
			</xsl:when>
			<xsl:when test="$Month=9">
				<xsl:value-of select="'SEP'"/>
			</xsl:when>
			<xsl:when test="$Month=10">
				<xsl:value-of select="'OCT'"/>
			</xsl:when>
			<xsl:when test="$Month=11">
				<xsl:value-of select="'NOV'"/>
			</xsl:when>
			<xsl:when test="$Month=12">
				<xsl:value-of select="'DEC'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>

	<xsl:template match="/ThirdPartyFlatFileDetailCollection">

		<ThirdPartyFlatFileDetailCollection>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedTP/Prana_Final.xsd</xsl:attribute>

			<ThirdPartyFlatFileDetail>

				<RowHeader>
					<xsl:value-of select ="'false'"/>
				</RowHeader>

				<Ordernumber>
					<xsl:value-of select="'Order number'"/>
				</Ordernumber>

				<Cancelcorrectindicator>
					<xsl:value-of select="'Cancel correct indicator'"/>
				</Cancelcorrectindicator>


				<Accountnumberoracronym>
					<xsl:value-of select="'Account number or acronym'"/>
				</Accountnumberoracronym>

				<Securityidentifier>
					<xsl:value-of select="'Security identifier'"/>
				</Securityidentifier>



				<Broker>
					<xsl:value-of select="'Broker'"/>
				</Broker>


				<Custodian>
					<xsl:value-of select="'Custodian'"/>
				</Custodian>

				<Transactiontype>
					<xsl:value-of select="'Transaction type'"/>
				</Transactiontype>

				<Currencycode>
					<xsl:value-of select="'Currency code'"/>
				</Currencycode>

				<Tradedate>
					<xsl:value-of select="'Trade Date'"/>
				</Tradedate>

				<Settledate>
					<xsl:value-of select="'Settle date'"/>
				</Settledate>

				<Quantity>
					<xsl:value-of select="'Quantity'"/>
				</Quantity>

				<Commission>
					<xsl:value-of select="'Commission'"/>
				</Commission>

				<Price>
					<xsl:value-of select="'Price'"/>
				</Price>

				<Accruedinterest>
					<xsl:value-of select="'Accrued interest'"/>
				</Accruedinterest>

				<Tradetax>
					<xsl:value-of select="'Trade tax'"/>
				</Tradetax>

				<Miscmoney >
					<xsl:value-of select="'Misc money'"/>
				</Miscmoney>

				<Netamount>
					<xsl:value-of select="'Net amount'"/>
				</Netamount>

				<Principal>
					<xsl:value-of select="'Principal'"/>
				</Principal>

				<Description>
					<xsl:value-of select="'Description'"/>
				</Description>


				<SecurityType>
					<xsl:value-of select="'Security Type'"/>
				</SecurityType>


				<CountrySettlementCode>
					<xsl:value-of select="'Country Settlement Code'"/>
				</CountrySettlementCode>

				<ClearingAgent>
					<xsl:value-of select="'Clearing Agent'"/>
				</ClearingAgent>

				<SECFees>
					<xsl:value-of select="'SEC Fees'"/>
				</SECFees>

				<Repoopensettledate>
					<xsl:value-of select="'Repo open settle date'"/>
				</Repoopensettledate>

				<RepoMaturityDate>
					<xsl:value-of select="'Repo Maturity Date'"/>
				</RepoMaturityDate>

				<RepoRate>
					<xsl:value-of select="'Repo Rate'"/>
				</RepoRate>

				<RepoInterest>
					<xsl:value-of select="'Repo Interest'"/>
				</RepoInterest>

				<Optionunderlyer>
					<xsl:value-of select="'Option underlyer'"/>
				</Optionunderlyer>

				<Optionexpirydate>
					<xsl:value-of select="'Optione xpiry date'"/>
				</Optionexpirydate>

				<Optioncallputindicator>
					<xsl:value-of select="'Option call put indicator'"/>
				</Optioncallputindicator>

				<Optionstrikeprice>
					<xsl:value-of select="'Options trike price'"/>
				</Optionstrikeprice>

				<Trailer>
					<xsl:value-of select="'Trailer'"/>
				</Trailer>

				<GenevaLotNumber1>
					<xsl:value-of select="'Geneva Lot Number 1'"/>
				</GenevaLotNumber1>

				<GainsKeeperLotNumber1>
					<xsl:value-of select="'Gains Keeper Lot Number 1'"/>
				</GainsKeeperLotNumber1>

				<LotDate1>
					<xsl:value-of select="'Lot Date 1'"/>
				</LotDate1>

				<LotQty1>
					<xsl:value-of select="'Lot Qty 1'"/>
				</LotQty1>

				<LotPrice1>
					<xsl:value-of select="'Lot Price 1'"/>
				</LotPrice1>

				<GenevaLotNumber2>
					<xsl:value-of select="'Geneva Lot Number 2'"/>
				</GenevaLotNumber2>

				<GainsKeeperLotNumber2>
					<xsl:value-of select="'Gains Keeper Lot Number 2'"/>
				</GainsKeeperLotNumber2>

				<LotDate2>
					<xsl:value-of select="'Lot Date 2'"/>
				</LotDate2>

				<LotQty2>
					<xsl:value-of select="'Lo tQty 2'"/>
				</LotQty2>

				<LotPrice2>
					<xsl:value-of select="'Lot Price 2'"/>
				</LotPrice2>

				<GenevaLotNumber3>
					<xsl:value-of select="'Geneva Lot Number 3'"/>
				</GenevaLotNumber3>

				<GainsKeeperLotNumber3>
					<xsl:value-of select="'GainsKeeper Lot Number 3'"/>
				</GainsKeeperLotNumber3>

				<LotDate3>
					<xsl:value-of select="'Lot Date 3'"/>
				</LotDate3>

				<LotQty3>
					<xsl:value-of select="'Lot Qty 3'"/>
				</LotQty3>

				<LotPrice3>
					<xsl:value-of select="'Lot Price 3'"/>
				</LotPrice3>

				<GenevaLotNumber4>
					<xsl:value-of select="'Geneva Lot Number 4'"/>
				</GenevaLotNumber4>

				<GainsKeeperLotNumber4>
					<xsl:value-of select="'Gains Keeper Lot Number 4'"/>
				</GainsKeeperLotNumber4>

				<LotDate4>
					<xsl:value-of select="'Lot Date 4'"/>
				</LotDate4>

				<LotQty4>
					<xsl:value-of select="'Lot Qty 4'"/>
				</LotQty4>

				<LotPrice4>
					<xsl:value-of select="'Lot Price 4'"/>
				</LotPrice4>

				<GenevaLotNumber5>
					<xsl:value-of select="'Geneva Lot Number 5'"/>
				</GenevaLotNumber5>

				<GainsKeeperLotNumber5>
					<xsl:value-of select="'Gains Keeper Lot Number 5'"/>
				</GainsKeeperLotNumber5>

				<LotDate5>
					<xsl:value-of select="'Lot Date 5'"/>
				</LotDate5>

				<LotQty5>
					<xsl:value-of select="'Lot Qty 5'"/>
				</LotQty5>

				<LotPrice5>
					<xsl:value-of select="'Lot Price 5'"/>
				</LotPrice5>

				<EntityID>
					<xsl:value-of select="EntityID"/>
				</EntityID>

			</ThirdPartyFlatFileDetail>


			<xsl:for-each select="ThirdPartyFlatFileDetail">
				<ThirdPartyFlatFileDetail>

					<RowHeader>
						<xsl:value-of select ="'false'"/>
					</RowHeader>

					<TaxLotState>
						<xsl:value-of select="TaxLotState"/>
					</TaxLotState>


					<Ordernumber>
						<xsl:value-of select="''"/>
					</Ordernumber>

					<Cancelcorrectindicator>
						<xsl:value-of select="''"/>
					</Cancelcorrectindicator>

					<xsl:variable name="PB_NAME" select="'GS'"/>

					<xsl:variable name = "PRANA_FUND_NAME">
						<xsl:value-of select="AccountName"/>
					</xsl:variable>

					<xsl:variable name ="THIRDPARTY_FUND_CODE">
						<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PranaFund=$PRANA_FUND_NAME]/@PBFundCode"/>
					</xsl:variable>
					<Accountnumberoracronym>
						<xsl:choose>
							<xsl:when test="$THIRDPARTY_FUND_CODE!=''">
								<xsl:value-of select="$THIRDPARTY_FUND_CODE"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_FUND_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</Accountnumberoracronym>

					<Securityidentifier>
						<xsl:choose>
							<xsl:when test ="ISIN!=''">
								<xsl:value-of select="ISIN"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="Symbol"/>
							</xsl:otherwise>
						</xsl:choose>
					</Securityidentifier>


					<xsl:variable name="PRANA_COUNTERPARTY_NAME" select="CounterParty"/>

					<xsl:variable name="THIRDPARTY_COUNTERPARTY_NAME">
						<xsl:value-of select="document('../ReconMappingXml/ThirdParty_ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$PB_NAME]/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY_NAME]/@ThirdPartyBrokerID"/>
					</xsl:variable>

					<xsl:variable name="Broker">
						<xsl:choose>
							<xsl:when test="$THIRDPARTY_COUNTERPARTY_NAME!=''">
								<xsl:value-of select="$THIRDPARTY_COUNTERPARTY_NAME"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_COUNTERPARTY_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					<Broker>
						<xsl:choose>
							<xsl:when test="CounterParty='BGCE'">
								<xsl:value-of select="'Merrill Lynch Broadcort'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
						
						<!--<xsl:value-of select="$Broker"/>-->
					</Broker>


					<Custodian>
						<xsl:value-of select="'GSCO'"/>
					</Custodian>

					<Transactiontype>
						<xsl:choose>
							<xsl:when test="Side='Buy'">
								<xsl:value-of select="'B'"/>
							</xsl:when>

							<xsl:when test="Side='Sell'">
								<xsl:value-of select="'S'"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</Transactiontype>

					<Currencycode>
						<xsl:value-of select="''"/>
					</Currencycode>

					<Tradedate>
						<xsl:value-of select="TradeDate"/>
					</Tradedate>

					<Settledate>
						<xsl:value-of select="SettlementDate"/>
					</Settledate>

					<Quantity>
						<xsl:choose>
							<xsl:when test ="number(AllocatedQty)">
								<xsl:value-of select="AllocatedQty"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</Quantity>


					<xsl:variable name="varCommission" select="CommissionCharged + SoftCommissionCharged"/>
					<Commission>
						<xsl:value-of select="format-number($varCommission,'##.##')"/>
					</Commission>

					<!--<Commission>
						--><!--<xsl:choose>
							<xsl:when test ="number(Commission)">
								<xsl:value-of select="format-number(Commission,'##.##')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>--><!--
						<xsl:value-of select="format-number(CommissionCharged,'##.##')"/>
					</Commission>-->

					<Price>
						<xsl:choose>
							<xsl:when test="number(AveragePrice)">
								<xsl:value-of select="format-number(AveragePrice,'####.####')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</Price>

					<Accruedinterest>
						<xsl:value-of select="''"/>
					</Accruedinterest>

					<Tradetax>
						<xsl:value-of select="''"/>
					</Tradetax>

					<Miscmoney >
						<xsl:value-of select="''"/>
					</Miscmoney>

					<Netamount>
						<xsl:choose>
							<xsl:when test="number(NetAmount)">
								<xsl:value-of select="format-number(NetAmount,'##.##')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</Netamount>

					<Principal>
						<xsl:value-of select="''"/>
					</Principal>

					<Description>
						<xsl:value-of select="''"/>
					</Description>


					<SecurityType>
						<xsl:value-of select="''"/>
					</SecurityType>


					<CountrySettlementCode>
						<xsl:value-of select="''"/>
					</CountrySettlementCode>

					<ClearingAgent>
						<xsl:value-of select="''"/>
					</ClearingAgent>

					<SECFees>
						<xsl:choose>
							<xsl:when test="number(SecFee)">
								<xsl:value-of select="format-number(SecFee,'#.00')"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</SECFees>

					<Repoopensettledate>
						<xsl:value-of select="''"/>
					</Repoopensettledate>

					<RepoMaturityDate>
						<xsl:value-of select="''"/>
					</RepoMaturityDate>

					<RepoRate>
						<xsl:value-of select="''"/>
					</RepoRate>

					<RepoInterest>
						<xsl:value-of select="''"/>
					</RepoInterest>

					<Optionunderlyer>
						<xsl:value-of select="''"/>
					</Optionunderlyer>

					<Optionexpirydate>
						<xsl:value-of select="''"/>
					</Optionexpirydate>

					<Optioncallputindicator>
						<xsl:value-of select="''"/>
					</Optioncallputindicator>

					<Optionstrikeprice>
						<xsl:value-of select="''"/>
					</Optionstrikeprice>

					<Trailer>
						<xsl:value-of select="''"/>
					</Trailer>

					<GenevaLotNumber1>
						<xsl:value-of select="''"/>
					</GenevaLotNumber1>

					<GainsKeeperLotNumber1>
						<xsl:value-of select="''"/>
					</GainsKeeperLotNumber1>

					<LotDate1>
						<xsl:value-of select="''"/>
					</LotDate1>

					<LotQty1>
						<xsl:value-of select="''"/>
					</LotQty1>

					<LotPrice1>
						<xsl:value-of select="''"/>
					</LotPrice1>

					<GenevaLotNumber2>
						<xsl:value-of select="''"/>
					</GenevaLotNumber2>

					<GainsKeeperLotNumber2>
						<xsl:value-of select="''"/>
					</GainsKeeperLotNumber2>

					<LotDate2>
						<xsl:value-of select="''"/>
					</LotDate2>

					<LotQty2>
						<xsl:value-of select="''"/>
					</LotQty2>

					<LotPrice2>
						<xsl:value-of select="''"/>
					</LotPrice2>

					<GenevaLotNumber3>
						<xsl:value-of select="''"/>
					</GenevaLotNumber3>

					<GainsKeeperLotNumber3>
						<xsl:value-of select="''"/>
					</GainsKeeperLotNumber3>

					<LotDate3>
						<xsl:value-of select="''"/>
					</LotDate3>

					<LotQty3>
						<xsl:value-of select="''"/>
					</LotQty3>

					<LotPrice3>
						<xsl:value-of select="''"/>
					</LotPrice3>

					<GenevaLotNumber4>
						<xsl:value-of select="''"/>
					</GenevaLotNumber4>

					<GainsKeeperLotNumber4>
						<xsl:value-of select="''"/>
					</GainsKeeperLotNumber4>

					<LotDate4>
						<xsl:value-of select="''"/>
					</LotDate4>

					<LotQty4>
						<xsl:value-of select="''"/>
					</LotQty4>

					<LotPrice4>
						<xsl:value-of select="''"/>
					</LotPrice4>

					<GenevaLotNumber5>
						<xsl:value-of select="''"/>
					</GenevaLotNumber5>

					<GainsKeeperLotNumber5>
						<xsl:value-of select="''"/>
					</GainsKeeperLotNumber5>

					<LotDate5>
						<xsl:value-of select="''"/>
					</LotDate5>

					<LotQty5>
						<xsl:value-of select="''"/>
					</LotQty5>

					<LotPrice5>
						<xsl:value-of select="''"/>
					</LotPrice5>

					<EntityID>
						<xsl:value-of select="EntityID"/>
					</EntityID>

				</ThirdPartyFlatFileDetail>

			</xsl:for-each>

		</ThirdPartyFlatFileDetailCollection>

	</xsl:template>

</xsl:stylesheet>