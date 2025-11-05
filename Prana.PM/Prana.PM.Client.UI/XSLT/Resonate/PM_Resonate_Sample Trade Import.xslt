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
			
			<xsl:if test="number(COL6)">
				
				<PositionMaster>

					<xsl:variable name = "var_PB_NAME" >
						<xsl:value-of select="'Morgan Stanley'"/>
					</xsl:variable>
					
					<xsl:variable name = "PB_FUND_NAME" >
						<xsl:value-of select="COL1"/>
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
						<xsl:value-of select="COL3"/>
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

					<PositionStartDate>
						<xsl:value-of select="COL8"/>
					</PositionStartDate>

					<SideTagValue>
						<xsl:choose>
							<xsl:when test="COL4 = 'Buy'">
								<xsl:value-of select="'1'"/>
							</xsl:when>
							<xsl:when test="COL4 = 'Sell'">
								<xsl:value-of select="'2'"/>
							</xsl:when>
														<xsl:when test="COL4 = 'Sell short'">
									<xsl:value-of select="'5'"/>
								</xsl:when>
								<xsl:when test="COL4 = 'Buy to Close'">
									<xsl:value-of select="'B'"/>
								</xsl:when>
							
							
								<xsl:when test="COL4 = 'Buy to Open'">
										<xsl:value-of select="'A'"/>
									</xsl:when>
									<xsl:when test="COL4 = 'Buy to Close'">
										<xsl:value-of select="'B'"/>
									</xsl:when>
							<xsl:when test="COL4 = 'Sell to Open'">
								<xsl:value-of select="'C'"/>
							</xsl:when>
							<xsl:when test="COL4 = 'Sell to Close'">
								<xsl:value-of select="'D'"/>
							</xsl:when>
									<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>
						</xsl:choose>
					</SideTagValue>					

					<NetPosition>
						<xsl:choose>
							<xsl:when  test="number(normalize-space(COL6)) &gt; 0">
								<xsl:value-of select="COL6"/>
							</xsl:when>
							<xsl:when test="number(normalize-space(COL6)) &lt; 0">
								<xsl:value-of select="COL6* (-1)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</NetPosition>

					<CostBasis>
						<xsl:choose>
							<xsl:when  test="number(normalize-space(COL7)) &gt; 0">
								<xsl:value-of select="COL7"/>
							</xsl:when>
							<xsl:when test="number(normalize-space(COL7)) &lt; 0">
								<xsl:value-of select="COL7* (-1)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</CostBasis>

					<PositionSettlementDate>
						<xsl:value-of select="COL9"/>
					</PositionSettlementDate>


					<Commission>
						<xsl:choose>
							<xsl:when  test="number(normalize-space(COL12)) &gt; 0">
								<xsl:value-of select="COL12"/>
							</xsl:when>
							<xsl:when test="number(normalize-space(COL12)) &lt; 0">
								<xsl:value-of select="COL12* (-1)"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</Commission>

					<xsl:variable name="OrfFee">
						<xsl:call-template name="Translate">
							<xsl:with-param name="Number" select="COL16"/>
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



					<xsl:variable name="SecFee">
						<xsl:call-template name="Translate">
							<xsl:with-param name="Number" select="COL13"/>
						</xsl:call-template>
					</xsl:variable>

					<SecFee>
						<xsl:choose>
							<xsl:when test="$SecFee &gt; 0">
								<xsl:value-of select="$SecFee"/>

							</xsl:when>
							<xsl:when test="$SecFee &lt; 0">
								<xsl:value-of select="$SecFee * (-1)"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>

						</xsl:choose>
					</SecFee>

					<xsl:variable name="StampDuty">
						<xsl:call-template name="Translate">
							<xsl:with-param name="Number" select="COL14"/>
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

					<xsl:variable name="Fees">
						<xsl:call-template name="Translate">
							<xsl:with-param name="Number" select="COL15"/>
						</xsl:call-template>
					</xsl:variable>

					<Fees>
						<xsl:choose>
							<xsl:when test="$Fees &gt; 0">
								<xsl:value-of select="$Fees"/>

							</xsl:when>
							<xsl:when test="$Fees &lt; 0">
								<xsl:value-of select="$Fees * (-1)"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>

						</xsl:choose>
					</Fees>

					<xsl:variable name="PB_COUNTER_PARTY" select="normalize-space(COL17)"/>

					<xsl:variable name="PRANA_COUNTER_PARTY">
						<xsl:value-of select ="document('../ReconMappingXML/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name=$var_PB_NAME]/BrokerData[@PranaBroker=$PB_COUNTER_PARTY]/@PranaBrokerCode"/>
					</xsl:variable>

					<CounterPartyID>
						<xsl:choose>

							<xsl:when test ="number($PRANA_COUNTER_PARTY) ">
								<xsl:value-of select ="$PRANA_COUNTER_PARTY"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select ="0"/>
							</xsl:otherwise>

						</xsl:choose>
					</CounterPartyID>


					<OriginalPurchaseDate>
						<xsl:value-of select="COL10"/>
					</OriginalPurchaseDate>

					<!--<TransactionType>
						<xsl:value-of select="COL5"/>
					</TransactionType>-->
					
				</PositionMaster>
				
			</xsl:if>
			
		</xsl:for-each>
		
	</DocumentElement>
	
 </xsl:template>

	<xsl:variable name="varLower" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="varUpper" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
</xsl:stylesheet> 
