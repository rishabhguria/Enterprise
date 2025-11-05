<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="//PositionMaster">
				<xsl:if test ="number(COL11) ">
				<PositionMaster>

					<!--<AccountName>
            <xsl:value-of select="''"/>
          </AccountName>-->
					
						
						
            		<Symbol>
						<xsl:value-of select="COL4"/>
					</Symbol>

					<PBSymbol>
						<xsl:value-of select="COL4"/>
					</PBSymbol>


					<xsl:variable name = "PB_FUND_NAME">
						<xsl:value-of select="''"/>
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
								<xsl:value-of select ="'JAT'"/>
							</xsl:otherwise>

						</xsl:choose>
					</AccountName>




					<xsl:choose>
						<xsl:when test ="COL11 &lt; 0">
							<NetPosition>
								<xsl:value-of select="COL11 *(-1)"/>
							</NetPosition>
						</xsl:when >
						<xsl:when test ="COL11 &gt; 0">
							<NetPosition>
								<xsl:value-of select="COL11"/>
							</NetPosition>
						</xsl:when >
						<xsl:otherwise>
							<NetPosition>
								<xsl:value-of select="0"/>
							</NetPosition>
						</xsl:otherwise>
					</xsl:choose >



					<xsl:variable name="Side" select="normalize-space(COL8)"/>


					<SideTagValue>
						<xsl:choose>
						
							<xsl:when test="$Side='B'">
								<xsl:value-of select="'1'"/>
							</xsl:when>
							<xsl:when test="$Side='S'">
								<xsl:value-of select="'2'"/>
							</xsl:when>

							<xsl:when test="$Side='Cover Short'or $Side='Cover'">
								<xsl:value-of select="'B'"/>
							</xsl:when>
							<xsl:when test="$Side='SS' or $Side='Short'">
								<xsl:value-of select="'5'"/>
							</xsl:when>
							<xsl:when test="$Side='BC'">
								<xsl:value-of select="'B'"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:value-of select="''"/>
							</xsl:otherwise>

						</xsl:choose>
					</SideTagValue>

					<xsl:variable name="Cost">
						<xsl:value-of select="COL12"/>
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

					<xsl:choose>
						<xsl:when test="boolean(number(COL13))">
							<Commission>
								<xsl:value-of select="COL13"/>
							</Commission>
						</xsl:when>
						<xsl:otherwise>
							<Commission>
								<xsl:value-of select="0"/>
							</Commission>
						</xsl:otherwise>
					</xsl:choose>

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
					
					
					<xsl:variable name="PB_CounterParty" select="normalize-space(COL2)"/>
					<xsl:variable name="PRANA_CounterPartyCode">
						<xsl:value-of select="document('../ReconMappingXml/ExecBrokerMapping.xml')/BrokerMapping/PB[@Name='GS']/BrokerData[translate(@MLBroker,$vLowercaseChars_CONST,$vUppercaseChars_CONST)=$PB_CounterParty]/@PranaBrokerCode"/>
					</xsl:variable>

					<xsl:choose>
						<xsl:when test ="$PRANA_CounterPartyCode !=''">
							<CounterPartyID>
								<xsl:value-of select="$PRANA_CounterPartyCode"/>
							</CounterPartyID>
						</xsl:when>
						<xsl:otherwise>
							<CounterPartyID>
								<xsl:value-of select="26"/>
							</CounterPartyID>
						</xsl:otherwise>
					</xsl:choose>


					<PositionStartDate>
						<xsl:value-of select="COL9"/>
						<!--<xsl:value-of select="concat(substring-before(substring-after(COL2,'/'),'/'),'/',substring-before(COL2,'/'),'/',substring-after(substring-after(COL2,'/'),'/'))"/>-->
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
