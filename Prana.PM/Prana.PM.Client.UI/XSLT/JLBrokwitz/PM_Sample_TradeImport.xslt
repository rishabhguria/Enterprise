<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

	<xsl:template name="Translate">
		<xsl:param name="Number"/>
		<xsl:variable name="SingleQuote">'</xsl:variable>

		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''))"/>
		</xsl:variable>

		<xsl:choose>
			<xsl:when test="contains($Number,'(')">
				<xsl:value-of select="$varNumber*-1"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$varNumber"/>
			</xsl:otherwise>
		</xsl:choose>

		<!--<xsl:value-of select="translate(translate(translate($Value,'(',''),')',''),',','')"/>-->
	</xsl:template>

<xsl:template match="/">
	<DocumentElement>
		
		<xsl:for-each select="//PositionMaster">
			
			<xsl:if test="number(COL5)">
				
				<PositionMaster>

					<xsl:variable name = "var_PB_NAME" >
						<xsl:value-of select="'BTIG'"/>
					</xsl:variable>
					
					<xsl:variable name = "PB_FUND_NAME" >
						<xsl:value-of select="COL21"/>
					</xsl:variable>

					<xsl:variable name="PRANA_FUND_NAME">
						<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$var_PB_NAME]/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
					</xsl:variable>

					<AccountName>
						<xsl:choose>
							<xsl:when test="$PRANA_FUND_NAME=''">
								<xsl:value-of select="$PB_FUND_NAME"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select='$PRANA_FUND_NAME'/>
							</xsl:otherwise>
						</xsl:choose>
					</AccountName>

					<Description>
						<xsl:value-of select="COL22"/>
					</Description>

					<!--<Strategy>
						<xsl:value-of select="COL20"/>				
					</Strategy>-->
					<xsl:variable name="PB_Symbol">
						<xsl:value-of select = "translate(COL2,$varLower,$varUpper)"/> 
					</xsl:variable>
						
					<xsl:variable name="PRANA_SYMBOL_NAME">
						<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/MSPB[@Name=$var_PB_NAME]/SymbolData[@PBCompanyName=$PB_Symbol]/@PranaSymbol"/>
					</xsl:variable>

					<PBSymbol>
						<xsl:value-of select="COL2"/>
					</PBSymbol>

					<xsl:variable name="Asset">
						<xsl:choose>
							<xsl:when test="contains(COL2,'O:')">
								<xsl:value-of select="'EquityOpyion'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="'Equity'"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					
					<Symbol>
						<xsl:choose>
							<xsl:when test="$PRANA_SYMBOL_NAME=''">
								<xsl:value-of select="$PB_Symbol"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
							</xsl:otherwise>
						</xsl:choose>
					</Symbol>

					<xsl:variable name="PB_BROKER_NAME">
						<xsl:value-of select="COL23"/>
					</xsl:variable>
					<xsl:variable name="PRANA_BROKER_ID">
						<xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$var_PB_NAME]/BrokerData[@PranaBroker=$PB_BROKER_NAME]/@PranaBrokerCode"/>
					</xsl:variable>
					<CounterPartyID>
						<xsl:choose>
							<xsl:when test="number($PRANA_BROKER_ID)">
								<xsl:value-of select="$PRANA_BROKER_ID"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</CounterPartyID>

					<PositionStartDate>
						<xsl:value-of select="COL3"/>
					</PositionStartDate>

					<SideTagValue>
						<xsl:choose>
							<xsl:when test="COL1 = 'Buy'">
								<xsl:value-of select="'1'"/>
							</xsl:when>
							<xsl:when test="COL1 = 'Sell'">
								<xsl:value-of select="'2'"/>
							</xsl:when>
									<xsl:when test="COL1 = 'Sell short'">
									<xsl:value-of select="'5'"/>
								</xsl:when>
								<xsl:when test="COL1 = 'Buy to Close'">
									<xsl:value-of select="'B'"/>
								</xsl:when>
									<xsl:when test="COL1 = 'Buy to Open'">
										<xsl:value-of select="'A'"/>
									</xsl:when>
									<xsl:when test="COL1 = 'Buy to Close'">
										<xsl:value-of select="'B'"/>
									</xsl:when>
							<xsl:when test="COL1 = 'Sell to Open'">
								<xsl:value-of select="'C'"/>
							</xsl:when>
							<xsl:when test="COL1 = 'Sell to Close'">
								<xsl:value-of select="'D'"/>
							</xsl:when>
									<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</SideTagValue>					

					<NetPosition>
						<xsl:choose>
							<xsl:when  test="number(normalize-space(COL5)) &gt; 0">
								<xsl:value-of select="COL5"/>
							</xsl:when>
							<xsl:when test="number(normalize-space(COL5)) &lt; 0">
								<xsl:value-of select="COL5* (-1)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</NetPosition>

					<CostBasis>
						<xsl:choose>
							<xsl:when  test="number(normalize-space(COL17)) &gt; 0">
								<xsl:value-of select="COL17"/>
							</xsl:when>
							<xsl:when test="number(normalize-space(COL17)) &lt; 0">
								<xsl:value-of select="COL17* (-1)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</CostBasis>

					<PositionSettlementDate>
						<xsl:value-of select="COL4"/>
					</PositionSettlementDate>

					<!--<Description>
						<xsl:value-of select="COL18"/>
					</Description>-->


					<Commission>
						<xsl:choose>
							<xsl:when  test="number(normalize-space(COL14)) &gt; 0">
								<xsl:value-of select="COL14"/>
							</xsl:when>
							<xsl:when test="number(normalize-space(COL14)) &lt; 0">
								<xsl:value-of select="COL14* (-1)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</Commission>

					<LotId>
						<xsl:value-of select="COL10"/>
					</LotId>

					<!--<ExternalTransId>
						<xsl:value-of select="COL11"/>
					</ExternalTransId>-->

					<xsl:variable name="varFees">
						<xsl:choose>
							<xsl:when  test="number(normalize-space(COL15)) ">
								<xsl:value-of select="COL15"/>
							</xsl:when>							
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<xsl:variable name="varOtherFee">
						<xsl:choose>
							<xsl:when  test="number(normalize-space(COL16)) ">
								<xsl:value-of select="COL16"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<xsl:variable name="StampDuty">
						<xsl:call-template name="Translate">
							<xsl:with-param name="Number" select="COL15"/>
						</xsl:call-template>
					</xsl:variable>

					<StampDuty>
						<xsl:choose>
							<xsl:when test="$StampDuty &gt; 0">
								<xsl:value-of select="$StampDuty"/>
							</xsl:when>
							<xsl:when test="$StampDuty &lt; 0">
								<xsl:value-of select="$StampDuty * (-1)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</StampDuty>

					<xsl:variable name="OrfFee">
						<xsl:call-template name="Translate">
							<xsl:with-param name="Number" select="COL12"/>
						</xsl:call-template>
					</xsl:variable>

					<OrfFee>
						<xsl:choose>
							<xsl:when test="$OrfFee &gt; 0">
								<xsl:value-of select="$OrfFee"/>
							</xsl:when>
							<xsl:when test="$OrfFee &lt; 0">
								<xsl:value-of select="$OrfFee * (-1)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</OrfFee>

					<!--<OriginalPurchaseDate>
						<xsl:value-of select="COL19"/>
					</OriginalPurchaseDate>-->

					<!--<TransactionType>
						<xsl:value-of select="COL23"/>
					</TransactionType>-->
					
				</PositionMaster>
				
			</xsl:if>
			
		</xsl:for-each>
		
	</DocumentElement>
	
 </xsl:template>

	<xsl:variable name="varLower" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="varUpper" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet> 
