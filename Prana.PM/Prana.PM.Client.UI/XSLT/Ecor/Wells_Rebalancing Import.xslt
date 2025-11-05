<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template name="Translate">
		<xsl:param name="Number"/>
		<xsl:variable name="SingleQuote">'</xsl:variable>

		<xsl:variable name="varNumber">
			<xsl:value-of select="number(translate(translate(translate(translate(translate($Number,'(',''),')',''),',',''),$SingleQuote,''),'+',''))"/>
		</xsl:variable>

		<xsl:choose>
			<xsl:when test="contains($Number,'(')">
				<xsl:value-of select="$varNumber * (-1)"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select="$varNumber"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	<xsl:template match="/">

		<DocumentElement>

				<xsl:for-each select ="//PositionMaster">

					<xsl:variable name="Position">
						<xsl:call-template name="Translate">
							<xsl:with-param name="Number" select="(COL3)"/>
						</xsl:call-template>
					</xsl:variable>
				
				<xsl:if test="number($Position)">
				
					<PositionMaster>
            		<Symbol>
						<xsl:value-of select="COL1"/>
					</Symbol>

					<PBSymbol>
						<xsl:value-of select="COL1"/>
					</PBSymbol>


					<xsl:variable name = "PB_FUND_NAME">
						<xsl:value-of select="COL144"/>
					</xsl:variable>

					<xsl:variable name ="PRANA_FUND_NAME">
						<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='WELLS FARGO']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
					</xsl:variable>

					<AccountName>
						<xsl:choose>

							<xsl:when test ="$PRANA_FUND_NAME!=''">
								<xsl:value-of select ="$PRANA_FUND_NAME"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select ="''"/>
							</xsl:otherwise>

						</xsl:choose>
					</AccountName>




					<xsl:choose>
						<xsl:when test ="COL3 &lt; 0">
							<NetPosition>
								<xsl:value-of select="COL3 * (-1)"/>
							</NetPosition>
						</xsl:when >
						<xsl:when test ="COL3 &gt; 0">
							<NetPosition>
								<xsl:value-of select="COL3"/>
							</NetPosition>
						</xsl:when >
						<xsl:otherwise>
							<NetPosition>
								<xsl:value-of select="0"/>
							</NetPosition>
						</xsl:otherwise>
					</xsl:choose >


					<xsl:choose>
						<xsl:when test ="COL2='cover' or COL2='Buy to Close'">
							<SideTagValue>
								<xsl:value-of select="'B'"/>
							</SideTagValue>
						</xsl:when >
						<xsl:when test ="COL2='sell' or COL2='Sell'">
							<SideTagValue>
								<xsl:value-of select="'2'"/>
							</SideTagValue>
						</xsl:when >


						<xsl:when test ="COL2='buy' or COL2='Buy'">
							<SideTagValue>
								<xsl:value-of select="'1'"/>
							</SideTagValue>
						</xsl:when >
						<xsl:when test ="COL2='short' or COL2='Sell Short'">
							<SideTagValue>
								<xsl:value-of select="'5'"/>
							</SideTagValue>
						</xsl:when >
						<!--xsl:when test ="COL7='SS'">
							<SideTagValue>
								<xsl:value-of select="'5'"/>
							</SideTagValue>
						</xsl:when >
						<xsl:when test ="COL7='S'">
							<SideTagValue>
								<xsl:value-of select="'2'"/>
							</SideTagValue>
						</xsl:when >-->
						
						<!--<xsl:when test ="COL7='Buy to Close'">
							<SideTagValue>
								<xsl:value-of select="'B'"/>
							</SideTagValue>
						</xsl:when >
						<xsl:when test ="(COL7='Sell short' or COL7='Sell Short') and COL8= 'Equity'">
							<SideTagValue>
								<xsl:value-of select="'5'"/>
							</SideTagValue>
						</xsl:when >
						<xsl:when test ="COL7='Sell to Open'">
							<SideTagValue>
								<xsl:value-of select="'C'"/>
							</SideTagValue>
						</xsl:when >
						<xsl:when test ="COL7='Buy to Open'">
							<SideTagValue>
								<xsl:value-of select="'A'"/>
							</SideTagValue>
						</xsl:when >
						<xsl:when test ="COL7='Sell to Close'">
							<SideTagValue>
								<xsl:value-of select="'D'"/>
							</SideTagValue>
						</xsl:when >-->

						<xsl:otherwise>
							<SideTagValue>
								<xsl:value-of select="''"/>
							</SideTagValue>
						</xsl:otherwise>
					</xsl:choose >

					<!--<TransactionType>

						<xsl:choose>
							<xsl:when test ="COL7='Long'">
								
									<xsl:value-of select="'LongAddition'"/>
								
							</xsl:when >
							<xsl:when test ="COL7='sell'">
							<xsl:value-of select="'5'"/>								
							</xsl:when >

							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>

						</xsl:choose>

					</TransactionType>-->

					<xsl:variable name="Cost">
						
						<xsl:choose>
							<xsl:when test="COL4 &lt; 0">
								<xsl:value-of select="(COL4 * -1) div COL3 "/>
							</xsl:when>
							<xsl:when test="COL3 &lt; 0">
								<xsl:value-of select="COL4 div (COL3 * -1) "/>
							</xsl:when>
							
							<xsl:otherwise>
								<xsl:value-of select="COL4 div COL3"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>



					<xsl:choose>
						<xsl:when test ="boolean(number($Cost))">
							<CostBasis>
								<xsl:value-of select="$Cost"/>
							</CostBasis>
						</xsl:when>
						<xsl:otherwise>
							<CostBasis>
								<xsl:value-of select="0"/>
							</CostBasis>
						</xsl:otherwise>
					</xsl:choose>

					<!--<xsl:choose>
						<xsl:when test="boolean(number(COL100))">
							<Commission>
								<xsl:value-of select="COL100"/>
							</Commission>
						</xsl:when>
						<xsl:otherwise>
							<Commission>
								<xsl:value-of select="0"/>
							</Commission>
						</xsl:otherwise>
					</xsl:choose>-->

					<!--<xsl:choose>
						<xsl:when test="boolean(number(COL10))">
							<MiscFees>
								<xsl:value-of select="COL10"/>
							</MiscFees>
						</xsl:when>
						<xsl:otherwise>
							<MiscFees>
								<xsl:value-of select="0"/>
							</MiscFees>
						</xsl:otherwise>
					</xsl:choose>-->

					<!--<xsl:choose>
						<xsl:when test="boolean(number(COL102))">
							<Fees>
								<xsl:value-of select="COL102"/>
							</Fees>
						</xsl:when>
						<xsl:otherwise>
							<Fees>
								<xsl:value-of select="0"/>
							</Fees>
						</xsl:otherwise>
					</xsl:choose>-->

					<Strategy>
						<xsl:value-of select="'Oleg'"/>
					</Strategy>
					


					<!--<xsl:variable name="PB_CounterParty" select="normalize-space(COL9)"/>
					<xsl:variable name="PRANA_CounterPartyCode">
						<xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name='ML']/BrokerData[translate(@MLBroker,$vLowercaseChars_CONST,$vUppercaseChars_CONST)=$PB_CounterParty]/@PranaBrokerCode"/>
					</xsl:variable>

					<xsl:choose>
						<xsl:when test ="$PRANA_CounterPartyCode !=''">
							<CounterPartyID>
								<xsl:value-of select="$PRANA_CounterPartyCode"/>
							</CounterPartyID>
						</xsl:when>
						<xsl:otherwise>
							<CounterPartyID>
								<xsl:value-of select="24"/>
							</CounterPartyID>
						</xsl:otherwise>
					</xsl:choose>-->


					<PositionStartDate>
						<xsl:value-of select="COL5"/>
					</PositionStartDate>

					<!--<OriginalPurchaseDate>
						<xsl:value-of select="COL10"/>
					</OriginalPurchaseDate>-->

				</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
	<!-- variable declaration for lower to upper case -->

	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

	<!-- variable declaration for lower to upper case ENDs -->


</xsl:stylesheet>
