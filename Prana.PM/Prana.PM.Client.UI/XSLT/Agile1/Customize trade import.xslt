<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>
	<xsl:template match="/">
		<DocumentElement>
			<xsl:attribute name="xsi:noNamespaceSchemaLocation">C:/UpdatedPM/PM.xsd</xsl:attribute>
			<xsl:for-each select="//PositionMaster">
				<PositionMaster>

					<!--<AccountName>
            <xsl:value-of select="''"/>
          </AccountName>-->

            		<Symbol>
						<xsl:value-of select="COL8"/>
					</Symbol>

					<PBSymbol>
						<xsl:value-of select="COL7"/>
					</PBSymbol>


					<xsl:variable name = "PB_FUND_NAME">
						<xsl:value-of select="COL222"/>
					</xsl:variable>

					<xsl:variable name ="PRANA_FUND_NAME">
						<xsl:value-of select="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name='FIXTrade']/FundData[@PBFundCode=$PB_FUND_NAME]/@PranaFund"/>
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
					
					<xsl:variable name="AssetType">
						<xsl:choose>
							<xsl:when test="COL5='CFD'">
								<xsl:value-of select="'Swap'"/>
							</xsl:when>
							<!-- <xsl:when test="string-length(COL8 &gt; 20)"> -->
							<xsl:when test="contains(COL5,'Option')">
								<xsl:value-of select="'EquityOption'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="'Equity'"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>
					
					<xsl:if test="$AssetType='Swap'">
						<IsSwapped>
							<xsl:value-of select="1"/>
						</IsSwapped>
						<SwapDescription>
							<xsl:value-of select="'SWAP'"/>
						</SwapDescription>
						<DayCount>
							<xsl:value-of select="365"/>
						</DayCount>
						<ResetFrequency>
							<xsl:value-of select="'Monthly'"/>
						</ResetFrequency>
						<OrigTransDate>
							<!--
<xsl:call-template name="MonthCode">
<xsl:with-param name="varMonth" select="COL6"/>
</xsl:call-template>
-->
							<!--
<xsl:value-of select="concat(substring(COL24,5,2),'/',substring(COL24,7,2),'/',substring(COL24,1,4))"/>
-->
							<xsl:value-of select="COL8"/>
						</OrigTransDate>
						<xsl:variable name="varPreviousMonth">
							<xsl:value-of select="substring-before(COL8,'/')"/>
						</xsl:variable>
						<xsl:variable name="varPrevMonth">
						
							<xsl:choose>
								<xsl:when test="number($varPreviousMonth)=1">
									<xsl:value-of select="12"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$varPreviousMonth - 1"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<xsl:variable name="varYearNo">
							<xsl:value-of select="(COL8)"/>
						</xsl:variable>
						<xsl:variable name="varYear">
							<xsl:choose>
								<xsl:when test="number($varPrevMonth)=1">
									<xsl:value-of select="($varYearNo)-1"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$varYearNo"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>
						<FirstResetDate>
							<xsl:value-of select="concat($varPrevMonth,'/28/',$varYear)"/>
						</FirstResetDate>
					</xsl:if>
					
					<xsl:variable name="Symbol">
						<xsl:value-of select="COL8"/>
					</xsl:variable>



					<xsl:choose>
						<xsl:when test ="COL10 &lt; 0">
							<NetPosition>
								<xsl:value-of select="COL10*(-1)"/>
							</NetPosition>
						</xsl:when >
						<xsl:when test ="COL10 &gt; 0">
							<NetPosition>
								<xsl:value-of select="COL10"/>
							</NetPosition>
						</xsl:when >
						<xsl:otherwise>
							<NetPosition>
								<xsl:value-of select="0"/>
							</NetPosition>
						</xsl:otherwise>
					</xsl:choose >


					<xsl:choose>
					<xsl:when test ="COL10 &gt; 0  and contains(COL5,'BOND')">
							<SideTagValue>
								<xsl:value-of select="'A'"/>
							</SideTagValue>
						</xsl:when >
						<xsl:when test ="COL10 &lt; 0  and contains(COL5,'BOND')">
							<SideTagValue>
								<xsl:value-of select="'C'"/>
							</SideTagValue>
						</xsl:when >
						<!--<xsl:when test ="COL8 &gt; 0 ">
							<SideTagValue>
								<xsl:value-of select="'1'"/>
							</SideTagValue>
						</xsl:when >
						<xsl:when test ="COL8 &lt; 0 ">
							<SideTagValue>
								<xsl:value-of select="'5'"/>
							</SideTagValue>
					
						
						</xsl:when >-->
						<xsl:when test ="contains(COL6,'LONG')">
							<SideTagValue>
								<xsl:value-of select="'1'"/>
							</SideTagValue>
						</xsl:when >
						<xsl:when test ="contains(COL6,'SHORT')">
							<SideTagValue>
								<xsl:value-of select="'5'"/>
							</SideTagValue>
						</xsl:when >
						
						<!--<xsl:when test ="COL10='Buy to Close'">
							<SideTagValue>
								<xsl:value-of select="'B'"/>
							</SideTagValue>
						</xsl:when >
						<xsl:when test ="(COL10='Sell short' or COL10='Sell Short') and COL10= 'Equity'">
							<SideTagValue>
								<xsl:value-of select="'5'"/>
							</SideTagValue>
						</xsl:when >
						<xsl:when test ="COL10='Sell to Open'">
							<SideTagValue>
								<xsl:value-of select="'C'"/>
							</SideTagValue>
						</xsl:when >
						<xsl:when test ="COL10='Buy to Open'">
							<SideTagValue>
								<xsl:value-of select="'A'"/>
							</SideTagValue>
						</xsl:when >
						<xsl:when test ="COL10='Sell to Close'">
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

					<xsl:variable name="Cost">
						<xsl:choose>
							<xsl:when test="number(COL10)">
								<!--<xsl:value-of select="format-number(COL16, '#.##########')"/>-->
								<xsl:value-of select="COL11"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
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
						<xsl:when test="boolean(number(COL12))">
							<Commission>
								<xsl:value-of select="COL12"/>
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
					</xsl:choose>

					<xsl:choose>
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
					
					
					<xsl:variable name="PB_CounterParty" select="normalize-space(COL24)"/>
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
								<xsl:value-of select="0"/>
							</CounterPartyID>
						</xsl:otherwise>
					</xsl:choose>


					<!--<FXRate>
						<xsl:value-of select="COL5"/>
					</FXRate>-->


				<xsl:variable name="FXRate">
						<xsl:choose>
							<xsl:when test="number(COL10)">
								<xsl:value-of select="'1'"/>
							</xsl:when>
							<xsl:otherwise>
								<xsl:value-of select="0"/>
							</xsl:otherwise>
						</xsl:choose>
					</xsl:variable>

					<xsl:choose>
						<xsl:when test ="boolean(number($FXRate))">
							<FXRate>
								<xsl:value-of select="$FXRate"/>
							</FXRate>
						</xsl:when>
						<xsl:otherwise>
							<FXRate>
								<xsl:value-of select="0"/>
							</FXRate>
						</xsl:otherwise>
					</xsl:choose>
					
					<!--<OriginalPurchaseDate>
						<xsl:value-of select="COL1"/>
					</OriginalPurchaseDate>-->

					<PositionStartDate>
						<xsl:value-of select="COL16"/>
					</PositionStartDate>

				</PositionMaster>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>
	<!-- variable declaration for lower to upper case -->

	<xsl:variable name="vLowercaseChars_CONST" select="'abcdefghijklmnopqrstuvwxyz'"/>
	<xsl:variable name="vUppercaseChars_CONST" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>

	<!-- variable declaration for lower to upper case ENDs -->


</xsl:stylesheet>
