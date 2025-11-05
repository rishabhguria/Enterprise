<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/">
		<ThirdPartyFlatFileDetailCollection>
			<!-- Assuming that source ThirdPartyFlatFileDetail nodes come sorted by EntityID -->
			<!-- let's build a Group node for each different EntityID by   -->
			<!-- looping trough all the records...                         -->
			<xsl:for-each select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail">

				<xsl:if test="(position()=1 or ((preceding-sibling::*[1]/Symbol != Symbol) or (preceding-sibling::*[1]/Side != Side) or (preceding-sibling::*[1]/CounterParty != CounterParty)))">
					<!-- ...buid a Group for this node_id -->
					<xsl:call-template name="TaxLotIDBuilder">

						<xsl:with-param name="I_Symbol">
							<xsl:value-of select="Symbol" />
						</xsl:with-param>

						<xsl:with-param name="I_Side">
							<xsl:value-of select="Side" />
						</xsl:with-param>

						<xsl:with-param name="I_CounterParty">
							<xsl:value-of select="CounterParty" />
						</xsl:with-param>

						<xsl:with-param name="LowestBlockID">
							<xsl:value-of select="PBUniqueID" />
						</xsl:with-param>

					</xsl:call-template>
				</xsl:if>
			</xsl:for-each>
			
		</ThirdPartyFlatFileDetailCollection>
	</xsl:template>

	<xsl:template name="TaxLotIDBuilder">

		<xsl:param name="I_Symbol" />
		<xsl:param name="I_Side" />
		<xsl:param name="I_CounterParty" />
		<xsl:param name="LowestBlockID" />

		<xsl:variable name="CommissionSum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[(Symbol=$I_Symbol) and (Side=$I_Side) and (CounterParty=$I_CounterParty)][TaxLotState != 'Deleted']/CommissionCharged)"/>
		</xsl:variable>

		<xsl:variable name="QtySum">
			<xsl:value-of  select="sum(/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[(Symbol=$I_Symbol) and (Side=$I_Side) and (CounterParty=$I_CounterParty)][TaxLotState != 'Deleted']/AllocatedQty)"/>
		</xsl:variable>

		<xsl:variable name ="varRowHeaderValue">
			<xsl:value-of select="'false'"/>
		</xsl:variable>

		<xsl:variable name ="varIsCaptionChangeReqValue">
			<xsl:value-of select="true"/>
		</xsl:variable>

		<xsl:variable name ="varUniqueID">
			<xsl:value-of select="$LowestBlockID"/>
		</xsl:variable>		

		<xsl:for-each select="/ThirdPartyFlatFileDetailCollection/ThirdPartyFlatFileDetail[((Symbol=$I_Symbol) and (Side=$I_Side) and (CounterParty=$I_CounterParty)) and (AccountNo !='013101969' and AccountNo !='013425764' and AccountNo !='013216619')]">
			
			<ThirdPartyFlatFileDetail>
				
				<GroupAllocationReq>
					<xsl:value-of select ="'true'"/>
				</GroupAllocationReq>
				<FileHeader>
					<xsl:value-of select ="'true'"/>
				</FileHeader>
				<FileFooter>
					<xsl:value-of select ="'true'"/>
				</FileFooter>
				<RowHeader>
					<xsl:value-of select ="'false'"/>
				</RowHeader>

				<TaxlotState>
					<xsl:value-of select ="TaxLotState"/>
				</TaxlotState>

				<xsl:variable name ="varCurrency">
					<xsl:value-of select ="CurrencySymbol"/>
				</xsl:variable>

				<!-- Non Swap Accounts does not need to be converted in to USD so normalized rate should be 1 -->
				<xsl:variable name="NormalizedFxRate">
					<xsl:choose>
						<xsl:when test="ForexRate_Trade='' or ForexRate_Trade='0' or AccountNo = '002276293' or AccountNo = '002200079' or AccountNo = '002200640'  or AccountNo = '002201481'">
							<xsl:value-of select="1"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="ForexRate_Trade"/>
						</xsl:otherwise>
					</xsl:choose>
				</xsl:variable>

				<orderlabel>
					<xsl:value-of select="'ORDER'"/>
				</orderlabel>

				<premise>
					<xsl:value-of select ="'PB0n'"/>
				</premise>

				<PBUniqueID>
					<xsl:value-of select ="$LowestBlockID"/>
				</PBUniqueID>

				<order_number>
					<xsl:value-of select="$LowestBlockID"/>
				</order_number>

				<xsl:variable name = "varMthTrade" >
					<xsl:value-of select="substring-before(TradeDate,'/')"/>
				</xsl:variable>
				<xsl:variable name = "varDateYrTrade" >
					<xsl:value-of select="substring-after(TradeDate,'/')"/>
				</xsl:variable>
				<xsl:variable name = "varYRTrade" >
					<xsl:value-of select="substring-after($varDateYrTrade,'/')"/>
				</xsl:variable>
				<xsl:variable name = "varDtTrade" >
					<xsl:value-of select="substring-before($varDateYrTrade,'/')"/>
				</xsl:variable>
				<xsl:choose>
					<xsl:when test ="string-length($varMthTrade) &lt; 2 and string-length($varDtTrade) &lt; 2">
						<process_dt>
							<xsl:value-of select="concat($varYRTrade,'',concat('0',$varMthTrade),'',concat('0',$varDtTrade))"/>
						</process_dt>
						<trade_dt>
							<xsl:value-of select="concat($varYRTrade,'',concat('0',$varMthTrade),'',concat('0',$varDtTrade))"/>
						</trade_dt>
					</xsl:when>
					<xsl:when test ="string-length($varMthTrade) &lt; 2 and string-length($varDtTrade) = 2">
						<process_dt>
							<xsl:value-of select="concat($varYRTrade,'',concat('0',$varMthTrade),'',$varDtTrade)"/>
						</process_dt>
						<trade_dt>
							<xsl:value-of select="concat($varYRTrade,'',concat('0',$varMthTrade),'',$varDtTrade)"/>
						</trade_dt>
					</xsl:when>
					<xsl:when test ="string-length($varMthTrade) = 2 and string-length($varDtTrade) &lt; 2">
						<process_dt>
							<xsl:value-of select="concat($varYRTrade,'',$varMthTrade,'',concat('0',$varDtTrade))"/>
						</process_dt>
						<trade_dt>
							<xsl:value-of select="concat($varYRTrade,'',$varMthTrade,'',concat('0',$varDtTrade))"/>
						</trade_dt>
					</xsl:when>
					<xsl:otherwise>
						<process_dt>
							<xsl:value-of select="concat($varYRTrade,'',$varMthTrade,'',$varDtTrade)"/>
						</process_dt>
						<trade_dt>
							<xsl:value-of select="concat($varYRTrade,'',$varMthTrade,'',$varDtTrade)"/>
						</trade_dt>
					</xsl:otherwise>
				</xsl:choose>


				<xsl:variable name = "varMthSettle" >
					<xsl:value-of select="substring-before(SettlementDate,'/')"/>
				</xsl:variable>
				<xsl:variable name = "varDateYrSettle" >
					<xsl:value-of select="substring-after(SettlementDate,'/')"/>
				</xsl:variable>
				<xsl:variable name = "varYRSettle" >
					<xsl:value-of select="substring-after($varDateYrSettle,'/')"/>
				</xsl:variable>
				<xsl:variable name = "varDtSettle" >
					<xsl:value-of select="substring-before($varDateYrSettle,'/')"/>
				</xsl:variable>
				<xsl:choose>
					<xsl:when test ="string-length($varMthSettle) &lt; 2">
						<settle_dt>
							<xsl:value-of select="concat($varYRSettle,'',concat('0',$varMthSettle),'',$varDtSettle)"/>
						</settle_dt>
					</xsl:when>
					<xsl:otherwise>
						<settle_dt>
							<xsl:value-of select="concat($varYRSettle,'',$varMthSettle,'',$varDtSettle)"/>
						</settle_dt>
					</xsl:otherwise>
				</xsl:choose>


				<!-- Side Starts-->
				<xsl:choose>
					<xsl:when test="Side='Buy' or Side='Buy to Open' ">
						<trans_mne>
							<xsl:value-of select="'BUY'"/>
						</trans_mne>
					</xsl:when>
					<xsl:when test="Side='Buy to Cover' or Side='Buy to Close'">
						<trans_mne>
							<xsl:value-of select="'BCOV'"/>
						</trans_mne>
					</xsl:when>
					<xsl:when test="Side='Sell' or Side='Sell to Close'">
						<trans_mne>
							<xsl:value-of select="'SELL'"/>
						</trans_mne>
					</xsl:when>
					<xsl:when test="Side='Sell short' or Side='Sell to Open'">
						<trans_mne>
							<xsl:value-of select="'SSEL'"/>
						</trans_mne>
					</xsl:when>
					<xsl:otherwise >
						<trans_mne>
							<xsl:value-of select="' '"/>
						</trans_mne>
					</xsl:otherwise>
				</xsl:choose >

				<xsl:choose>
					<xsl:when test="TaxLotState='Amemded'">
						<activity_cd>
							<xsl:value-of select="'A'"/>
						</activity_cd>
					</xsl:when>
					<xsl:when test="TaxLotState='Canceled'">
						<activity_cd>
							<xsl:value-of select="'C'"/>
						</activity_cd>
					</xsl:when>
					<xsl:otherwise >
						<activity_cd>
							<xsl:value-of select="'N'"/>
						</activity_cd>
					</xsl:otherwise>
				</xsl:choose >

				<custodian_mne>
					<xsl:choose>
						<xsl:when test ="CurrencySymbol = 'NZD'">
							<xsl:value-of select="'DBNYNZ'"/>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select="'GSI'"/>
						</xsl:otherwise>
					</xsl:choose>
				</custodian_mne>

				<!--<prod_id>
						<xsl:value-of select ="CUSIP"/>
					</prod_id>

					<prod_id_type>
						<xsl:value-of select ="'C'"/>
					</prod_id_type>-->

				<prod_id>
					<xsl:value-of select="SEDOL"/>
				</prod_id>

				<prod_id_type>
					<xsl:value-of select="'S'"/>
				</prod_id_type>

				<prod_desc>
					<xsl:value-of select ="' '"/>
				</prod_desc>

				<!-- Starts Asset Type -->
				<xsl:choose>
					<xsl:when test="AccountNo='013425764' or AccountNo='013101969' or AccountNo='049031727'">
						<security_type>
							<xsl:value-of select="'SWAP'"/>
						</security_type>
					</xsl:when>
					<xsl:otherwise >
						<security_type>
							<xsl:value-of select="'EQ'"/>
						</security_type>
					</xsl:otherwise>
				</xsl:choose>
				<!--<xsl:choose>
						<xsl:when test="Asset='Equity'">
							<security_type>
								<xsl:value-of select="'EQ'"/>
							</security_type>
						</xsl:when>
						<xsl:when test="Asset='EquityOption'">
							<security_type>
								<xsl:value-of select="'OPT'"/>
							</security_type>
						</xsl:when>
						<xsl:when test="Asset='FX'">
							<security_type>
								<xsl:value-of select="'FX'"/>
							</security_type>
						</xsl:when>
						<xsl:when test="Asset='FixedIncome'">
							<security_type>
								<xsl:value-of select="'FI'"/>
							</security_type>
						</xsl:when>
						<xsl:otherwise >
							<security_type>
								<xsl:value-of select="' '"/>
							</security_type>
						</xsl:otherwise>
					</xsl:choose>-->

				<!--Ends Asset Type -->
				<exchange>
					<xsl:value-of select="' '"/>
				</exchange>

				<xsl:choose>
					<xsl:when test ="CounterParty='GSPrg' or CounterParty='GSElec'">
						<trading_cparty_mne>
							<xsl:value-of select="'GS'"/>
						</trading_cparty_mne>
					</xsl:when>
					<xsl:when test ="CounterParty='UBS Program' or CounterParty='UBS Electronic'">
						<trading_cparty_mne>
							<xsl:value-of select="'UBSW'"/>
						</trading_cparty_mne>
					</xsl:when>
					<xsl:when test ="(CounterParty='DBPrg' or CounterParty='DBElec') and CurrencySymbol = 'NZD'">
						<trading_cparty_mne>
							<xsl:value-of select="'DBNT'"/>
						</trading_cparty_mne>
					</xsl:when>
					<xsl:when test ="(CounterParty='DBPrg' or CounterParty='DBElec')and CurrencySymbol != 'NZD'">
						<trading_cparty_mne>
							<xsl:value-of select="'DB'"/>
						</trading_cparty_mne>
					</xsl:when>
					<xsl:when test ="CounterParty='CS Program' or CounterParty='CSElect'">
						<trading_cparty_mne>
							<xsl:value-of select="'CSFBTC'"/>
						</trading_cparty_mne>
					</xsl:when>
					<xsl:when test ="CounterParty='INSTElec' or CounterParty='DBElec'">
						<trading_cparty_mne>
							<xsl:value-of select="'INSTINET'"/>
						</trading_cparty_mne>
					</xsl:when>
					<xsl:otherwise >
						<trading_cparty_mne>
							<xsl:value-of select="CounterParty"/>
						</trading_cparty_mne>
					</xsl:otherwise>
				</xsl:choose >


				<!--<xsl:variable name = "PRANA_COUNTERPARTY" >
						<xsl:value-of select="CounterParty"/>
					</xsl:variable>
					<xsl:variable name="PB_COUNTERPARTY">
						<xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name='GS']/BrokerData[@PranaBroker=$PRANA_COUNTERPARTY]/@GSBroker"/>
					</xsl:variable>-->

				<!--<xsl:choose>
						<xsl:when test="$PB_COUNTERPARTY=''">
							<trading_cparty_mne>
								<xsl:value-of select ="$PRANA_COUNTERPARTY"/>
							</trading_cparty_mne>							
						</xsl:when>
						<xsl:otherwise>
							<trading_cparty_mne>
								<xsl:value-of select ="$PB_COUNTERPARTY"/>
							</trading_cparty_mne>
						</xsl:otherwise>
					</xsl:choose>-->
				<!--<xsl:choose>
						<xsl:when test ="CounterParty='BMCI'">
							<trading_cparty_mne>
								<xsl:value-of select="'YORKTON'"/>
							</trading_cparty_mne>
						</xsl:when>
						<xsl:when test ="CounterParty='Bofa'">
							<trading_cparty_mne>
								<xsl:value-of select="'BOFA'"/>
							</trading_cparty_mne>
						</xsl:when>
						<xsl:when test ="CounterParty='Cant'">
							<trading_cparty_mne>
								<xsl:value-of select="'CANTOR'"/>
							</trading_cparty_mne>
						</xsl:when>
						<xsl:when test ="CounterParty='CIBC'">
							<trading_cparty_mne>
								<xsl:value-of select="'CIBC'"/>
							</trading_cparty_mne>
						</xsl:when>
						<xsl:when test ="CounterParty='CICF'">
							<trading_cparty_mne>
								<xsl:value-of select="'CANACORDCAP'"/>
							</trading_cparty_mne>
						</xsl:when>
						<xsl:when test ="CounterParty='CRT'">
							<trading_cparty_mne>
								<xsl:value-of select="'CREDRES'"/>
							</trading_cparty_mne>
						</xsl:when>
						<xsl:when test ="CounterParty='CSFB'">
							<trading_cparty_mne>
								<xsl:value-of select="'CSFB'"/>
							</trading_cparty_mne>
						</xsl:when>
						<xsl:when test ="CounterParty='Execution'">
							<trading_cparty_mne>
								<xsl:value-of select="'EXECUTION'"/>
							</trading_cparty_mne>
						</xsl:when>
						<xsl:when test ="CounterParty='FECC'">
							<trading_cparty_mne>
								<xsl:value-of select="'FIRSTENERGY'"/>
							</trading_cparty_mne>
						</xsl:when>
						<xsl:when test ="CounterParty='Griswold'">
							<trading_cparty_mne>
								<xsl:value-of select="'GRISWOLD'"/>
							</trading_cparty_mne>
						</xsl:when>
						<xsl:when test ="CounterParty='GS'">
							<trading_cparty_mne>
								<xsl:value-of select="'GSCO'"/>
							</trading_cparty_mne>
						</xsl:when>
						<xsl:when test ="CounterParty='GS1'">
							<trading_cparty_mne>
								<xsl:value-of select="'GSNY'"/>
							</trading_cparty_mne>
						</xsl:when>
						<xsl:when test ="CounterParty='ITG'">
							<trading_cparty_mne>
								<xsl:value-of select="'ITG'"/>
							</trading_cparty_mne>
						</xsl:when>
						<xsl:when test ="CounterParty='Labs'">
							<trading_cparty_mne>
								<xsl:value-of select="'LABR'"/>
							</trading_cparty_mne>
						</xsl:when>
						<xsl:when test ="CounterParty='LEHM'">
							<trading_cparty_mne>
								<xsl:value-of select="'LEHM'"/>
							</trading_cparty_mne>
						</xsl:when>
						<xsl:when test ="CounterParty='Maxim'">
							<trading_cparty_mne>
								<xsl:value-of select="'MAXWELL'"/>
							</trading_cparty_mne>
						</xsl:when>
						<xsl:when test ="CounterParty='Marrion'">
							<trading_cparty_mne>
								<xsl:value-of select="'MERRION'"/>
							</trading_cparty_mne>
						</xsl:when>
						<xsl:when test ="CounterParty='Merrion'">
							<trading_cparty_mne>
								<xsl:value-of select="'MERRION'"/>
							</trading_cparty_mne>
						</xsl:when>
						<xsl:when test ="CounterParty='MLCO'">
							<trading_cparty_mne>
								<xsl:value-of select="'MLCO'"/>
							</trading_cparty_mne>
						</xsl:when>
						<xsl:when test ="CounterParty='MSCO'">
							<trading_cparty_mne>
								<xsl:value-of select="'MSCO'"/>
							</trading_cparty_mne>
						</xsl:when>
						<xsl:when test ="CounterParty='Pension'">
							<trading_cparty_mne>
								<xsl:value-of select="'PENSION'"/>
							</trading_cparty_mne>
						</xsl:when>
						<xsl:when test ="CounterParty='PETE'">
							<trading_cparty_mne>
								<xsl:value-of select="'PETE'"/>
							</trading_cparty_mne>
						</xsl:when>
						<xsl:when test ="CounterParty='RAJA'">
							<trading_cparty_mne>
								<xsl:value-of select="'RAJA'"/>
							</trading_cparty_mne>
						</xsl:when>
						<xsl:when test ="CounterParty='SCMC'">
							<trading_cparty_mne>
								<xsl:value-of select="'SCMC'"/>
							</trading_cparty_mne>
						</xsl:when>
						<xsl:when test ="CounterParty='TDSI'">
							<trading_cparty_mne>
								<xsl:value-of select="'TDSI'"/>
							</trading_cparty_mne>
						</xsl:when>
						<xsl:when test ="CounterParty='Tris'">
							<trading_cparty_mne>
								<xsl:value-of select="'TRIS'"/>
							</trading_cparty_mne>
						</xsl:when>
						<xsl:when test ="CounterParty='Wach'">
							<trading_cparty_mne>
								<xsl:value-of select="'FUCM'"/>
							</trading_cparty_mne>
						</xsl:when>
						<xsl:when test ="CounterParty='Weed'">
							<trading_cparty_mne>
								<xsl:value-of select="'WEED'"/>
							</trading_cparty_mne>
						</xsl:when>
						<xsl:when test ="CounterParty='GSEC'">
							<trading_cparty_mne>
								<xsl:value-of select="'REDI'"/>
							</trading_cparty_mne>
						</xsl:when>
						<xsl:otherwise >
							<trading_cparty_mne>
								<xsl:value-of select="CounterParty"/>
							</trading_cparty_mne>
						</xsl:otherwise>
					</xsl:choose >-->

				<pricing_ccy>
					<xsl:value-of select="''"/>
				</pricing_ccy>

				<xsl:choose>
					<xsl:when test="AccountNo='013425764' or AccountNo='013101969' or AccountNo='049031727'">
						<settlement_ccy>
							<xsl:value-of select="'USD'"/>
						</settlement_ccy>
					</xsl:when>
					<xsl:otherwise >
						<settlement_ccy>
							<xsl:value-of select="CurrencySymbol"/>
						</settlement_ccy>
					</xsl:otherwise>
				</xsl:choose >

				<xsl:choose>
					<xsl:when test ="CounterParty='GSPrg' or CounterParty='GSElec'">
						<order_price>
							<xsl:value-of select='format-number(AveragePrice*$NormalizedFxRate, "0.000000")'/>
						</order_price>
					</xsl:when>
					<xsl:otherwise >
						<order_price>
							<xsl:value-of select='format-number(AveragePrice*$NormalizedFxRate, "0.0000")'/>
						</order_price>
					</xsl:otherwise>
				</xsl:choose >

				<orderqty>
					<!--<xsl:value-of select="ExecutedQty"/>-->
					<xsl:value-of select="$QtySum"/>
					
				</orderqty>

				<principal>
					<!--format-number($QtySum * AveragePrice, "###") '$QtySum * AveragePrice'-->
					<!--<xsl:value-of select='format-number($QtySum * AveragePrice, "###.00")'/>-->
					<xsl:value-of select='format-number($QtySum * format-number(AveragePrice, "###.0000"), "###.00")'/>
				</principal>

				<net_amt_price_ccy>
					<xsl:choose>
						<xsl:when test ="NetAmount != 0">
							<xsl:choose>
								<xsl:when test ="CurrencySymbol = 'JPY'">
									<xsl:value-of select='format-number(NetAmount, "###")'/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select='format-number(NetAmount, "###.00")'/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:when>
						<xsl:otherwise>
							<xsl:value-of select ="NetAmount"/>
						</xsl:otherwise>
					</xsl:choose>
				</net_amt_price_ccy>

				<!--<NetCashAmount>
						<xsl:choose>
							<xsl:when test ="NetAmount != 0">
								<xsl:choose>
									<xsl:when test ="CurrencySymbol = 'JPY'">
										<xsl:value-of select='format-number(NetAmount, "###")'/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select='format-number(NetAmount, "###.00")'/>
									</xsl:otherwise>
								</xsl:choose>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select ="NetAmount"/>
							</xsl:otherwise>
						</xsl:choose>
					</NetCashAmount>-->


				<net_amt_settle_ccy>
					<xsl:value-of select ="'0'"/>
				</net_amt_settle_ccy>

				<exchange_rate>
					<xsl:value-of select ="'0'"/>
				</exchange_rate>

				<exchange_rate_type>
					<xsl:value-of select ="' '"/>
				</exchange_rate_type>

				<total_sec_fees>
					<xsl:value-of select ="'0'"/>
				</total_sec_fees>
				<total_taxes>
					<xsl:value-of select ='format-number(StampDuty, "####.0000")*$NormalizedFxRate'/>
				</total_taxes>
				<total_other_charges>
					<xsl:value-of select='format-number(OtherBrokerFee + MiscFees + TransactionLevy + ClearingFee , "###.0000")*$NormalizedFxRate'/>
				</total_other_charges>
				<total_commission>
					<xsl:value-of select ='format-number(CommissionCharged + TaxOnCommissions, "###.0000")*$NormalizedFxRate'/>
				</total_commission>

				<accrued_interest>
					<xsl:value-of select ="'0'"/>
				</accrued_interest>

				<agency_principal_ind>
					<xsl:value-of select ="' '"/>
				</agency_principal_ind>

				<clr_agent>
					<xsl:value-of select ="' '"/>
				</clr_agent>

				<country_code>
					<xsl:value-of select ="' '"/>
				</country_code>

				<overide_delvy_instruct>
					<xsl:value-of select ="' '"/>
				</overide_delvy_instruct>

				<pair_off_prod_id>
					<xsl:value-of select ="' '"/>
				</pair_off_prod_id>

				<repo_open_settle_dt>
					<xsl:value-of select ="' '"/>
				</repo_open_settle_dt>

				<repo_maturity_dt>
					<xsl:value-of select ="' '"/>
				</repo_maturity_dt>

				<repo_collateral_qty>
					<xsl:value-of select ="'0'"/>
				</repo_collateral_qty>

				<repo_rate>
					<xsl:value-of select ="'0'"/>
				</repo_rate>

				<repo_interest>
					<xsl:value-of select ="'0'"/>
				</repo_interest>

				<repo_close_amount>
					<xsl:value-of select ="'0'"/>
				</repo_close_amount>

				<repo_coupon_amt>
					<xsl:value-of select ="'0'"/>
				</repo_coupon_amt>

				<open_close_ind>
					<xsl:value-of select ="' '"/>
				</open_close_ind>

				<free_text1>
					<xsl:value-of select ="' '"/>
				</free_text1>

				<GroupEnds>
					<xsl:value-of select ="'GroupEnds'"/>
				</GroupEnds>

				<Allocat>
					<xsl:value-of select ="'ALLOCAT'"/>
				</Allocat>

				<AllocationId>
					<xsl:value-of select ="TradeRefID"/>
				</AllocationId>

				<trading_acc_mne>
					<xsl:value-of select="AccountNo"/>
				</trading_acc_mne>

				<order_qty>
					<xsl:value-of select ="AllocatedQty"/>
				</order_qty>

				<Allocent>
					<xsl:value-of select ="'ALLOCENT'"/>
				</Allocent>


				<!--application internal use only -->

				<EntityID>
					<xsl:value-of select="EntityID"/>
				</EntityID>

			</ThirdPartyFlatFileDetail>
		</xsl:for-each>

	</xsl:template>
</xsl:stylesheet>
