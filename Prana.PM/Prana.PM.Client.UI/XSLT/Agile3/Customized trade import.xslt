<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    >
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
						<xsl:with-param name="Number" select="(COL6)"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($Position) and not(contains(COL6,'Currency')) ">
				
					<PositionMaster>
					<xsl:variable name="smallcase" select="'abcdefghijklmnopqrstuvwxyz'" />
					<xsl:variable name="uppercase" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'" />

            		<Symbol>
						<!--<xsl:value-of select="COL7"/>-->
						<xsl:value-of select="translate(COL2, $smallcase, $uppercase)" />
						<!--<xsl:value-of select="substring-before(COL3,' ')"/>-->
					</Symbol>

					<PBSymbol>
						<xsl:value-of select="COL1"/>
					</PBSymbol>


					<xsl:variable name = "PB_FUND_NAME">
						<xsl:value-of select="COL116"/>
					</xsl:variable>

					<xsl:variable name ="PRANA_FUND_NAME">
						<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='UBS']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
					</xsl:variable>

					<AccountName>
						<xsl:choose>

							<xsl:when test ="$PRANA_FUND_NAME!=''">
								<xsl:value-of select ="$PRANA_FUND_NAME"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select ="COL55"/>
							</xsl:otherwise>

						</xsl:choose>
					</AccountName>




					<xsl:choose>
						<xsl:when test ="COL6 &lt; 0">
							<NetPosition>
								<xsl:value-of select="COL6 *(-1)"/>
							</NetPosition>
						</xsl:when >
						<xsl:when test ="COL6 &gt; 0">
							<NetPosition>
								<xsl:value-of select="COL6"/>
							</NetPosition>
						</xsl:when >
						<xsl:otherwise>
							<NetPosition>
								<xsl:value-of select="0"/>
							</NetPosition>
						</xsl:otherwise>
					</xsl:choose >



					<xsl:variable name="Side" select="normalize-space(COL5)"/>


					<SideTagValue>
						<xsl:choose>
						
							<xsl:when test="$Side='BUY'">
								<xsl:value-of select="'1'"/>
							</xsl:when>
							<xsl:when test="$Side='SELL'">
								<xsl:value-of select="'2'"/>
							</xsl:when>

							<xsl:when test="$Side='BUY TO COVER'">
								<xsl:value-of select="'B'"/>
							</xsl:when>
							<xsl:when test="$Side='SHORT SELL'">
								<xsl:value-of select="'5'"/>
							</xsl:when>
							<xsl:when test="$Side='BUY TO OPEN'">
								<xsl:value-of select="'A'"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>

						</xsl:choose>
					</SideTagValue>
					
						

					<xsl:variable name="Cost">
						<xsl:value-of select="COL13"/>
					</xsl:variable>


						<!--<TransactionType>
							<xsl:choose>
								<xsl:when test ="COL6 &gt; 0 ">
									<xsl:value-of select="'LongAddition'"/>
								</xsl:when>
								<xsl:when test ="COL6 &lt; 0 ">
									<xsl:value-of select="'ShortAddition'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</TransactionType>-->
						

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
						<xsl:when test="boolean(number(COL11))">
							<Commission>
								<xsl:value-of select="COL11"/>
							</Commission>
						</xsl:when>
						<xsl:otherwise>
							<Commission>
								<xsl:value-of select="0"/>
							</Commission>
						</xsl:otherwise>
					</xsl:choose>-->

					<!--<xsl:choose>
						<xsl:when test="boolean(number(COL11))">
							<MiscFees>
								<xsl:value-of select="COL11"/>
							</MiscFees>
						</xsl:when>
						<xsl:otherwise>
							<MiscFees>
								<xsl:value-of select="0"/>
							</MiscFees>
						</xsl:otherwise>
					</xsl:choose>-->

					<!--<xsl:choose>
						<xsl:when test="boolean(number(COL12))">
							<Fees>
								<xsl:value-of select="COL12"/>
							</Fees>
						</xsl:when>
						<xsl:otherwise>
							<Fees>
								<xsl:value-of select="0"/>
							</Fees>
						</xsl:otherwise>
					</xsl:choose>-->
					
					
					<xsl:variable name="PB_CounterParty" select="substring-before(COL9,' ')"/>
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
								<xsl:value-of select="63"/>
							</CounterPartyID>
						</xsl:otherwise>
					</xsl:choose>


					<PositionStartDate>
						<xsl:value-of select="COL3"/>
						<!--<xsl:value-of select="concat(substring-before(substring-after(COL9,'/'),'/'),'/',substring-before(COL9,'/'),'/',substring-after(substring-after(COL9,'/'),'/'))"/>-->
					</PositionStartDate>

					<!--<OriginalPurchaseDate>
						<xsl:value-of select="COL1"/>
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
