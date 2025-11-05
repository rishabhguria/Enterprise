<?xml version="1.0" encoding="utf-8"?>

<xsl:stylesheet version="1.0"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
	xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">

	<xsl:variable name = "PB_NAME">
		<xsl:value-of select="'Touradji'"/>
	</xsl:variable>

	<xsl:variable name = "PB_SYMBOL_NAME" >
		<xsl:value-of select ="COL29"/>
	</xsl:variable>

	<msxsl:script language="C#" implements-prefix="my">

		public string Now1(int year, int month)
		{
		DateTime thirdWednesday= new DateTime(year, month, 15);
		while (thirdWednesday.DayOfWeek != DayOfWeek.Wednesday)
		{
		thirdWednesday = thirdWednesday.AddDays(1);
		}
		return thirdWednesday.ToString();
		}

	</msxsl:script>
	
	<xsl:template name="MonthCode">
		<xsl:param name="varMonth"/>

		<!-- Call month Codes e.g. 01 represents Call,January...  13 put january -->
		<xsl:choose>
			<xsl:when test ="$varMonth='01' ">
				<xsl:value-of select ="'F'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='02' ">
				<xsl:value-of select ="'G'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='03'">
				<xsl:value-of select ="'H'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='04' ">
				<xsl:value-of select ="'J'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='05' ">
				<xsl:value-of select ="'K'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='06'">
				<xsl:value-of select ="'M'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='07' ">
				<xsl:value-of select ="'N'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='08' ">
				<xsl:value-of select ="'Q'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='09' ">
				<xsl:value-of select ="'U'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='10' ">
				<xsl:value-of select ="'V'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='11'">
				<xsl:value-of select ="'X'"/>
			</xsl:when>
			<xsl:when test ="$varMonth='12'">
				<xsl:value-of select ="'Z'"/>
			</xsl:when>
			<xsl:otherwise>
				<xsl:value-of select ="''"/>
			</xsl:otherwise>
		</xsl:choose>
	</xsl:template>
	
	<xsl:template name="Symbol">		

		<xsl:variable name="PRANA_SYMBOL_NAME">
			<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
		</xsl:variable>

		<xsl:variable name="varUnderlyingFUTLME">
			<xsl:value-of select="substring(COL20,3,3)"/>
		</xsl:variable>

		<xsl:variable name="varUnderlyingFUTOption">
			<xsl:value-of select="substring(COL20,1,2)"/>
		</xsl:variable>

		<xsl:variable name="varYear" >
			<xsl:choose>
				<xsl:when test="not(contains(COL22,'LME'))">
					<xsl:value-of select="substring(COL20,4,1)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="substring(substring-after(COL20,' '),4,1)"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varMonthCode">
			<xsl:choose>
				<xsl:when test="not(contains(COL22,'LME'))">
					<xsl:value-of select="substring(COL20,3,1)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:call-template name="MonthCode">
						<xsl:with-param name="varMonth" select="substring(substring-after(COL20,' '),5,2)"/>
					</xsl:call-template>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varDate" select="substring(substring-after(COL20,' '),7,2)"/>

		<xsl:variable name="varStrikePrice" select="substring-after(COL20,' ')"/>

		<xsl:variable name="PB_CODE" select="COL17"/>

		<xsl:variable name="PB_FLAG" select="COL18"/>

		<xsl:variable name="PRANA_UNDERLYING_NAME">
			<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCode=$PB_CODE and @YellowFlag=$PB_FLAG]/@UnderlyingCode"/>
		</xsl:variable>

		<xsl:variable name="PRANA_EXCHANGE_NAME">
			<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCode=$PB_CODE and @YellowFlag=$PB_FLAG]/@ExchangeCode"/>
		</xsl:variable>
		
		<xsl:variable name="PRANA_FLAG">
			<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCode=$PB_CODE and @YellowFlag=$PB_FLAG]/@ExpFlag"/>
		</xsl:variable>


		<xsl:variable name="PRANA_STRIKE_MULTIPLIER">
			<xsl:value-of select="document('../ReconMappingXml/UnderlyingCodeMapping.xml')/UnderlyingMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCode=$PB_CODE and @YellowFlag=$PB_FLAG]/@StrikeMul"/>
		</xsl:variable>

		<xsl:variable name="StrikePrice">
			<xsl:choose>
				<xsl:when test="$PRANA_STRIKE_MULTIPLIER!=''">
					<xsl:value-of select="$varStrikePrice*$PRANA_STRIKE_MULTIPLIER"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$varStrikePrice"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varFUTOPTUnderlying">
			<xsl:choose>
				<xsl:when test="$PRANA_UNDERLYING_NAME!=''">
					<xsl:value-of select="$PRANA_UNDERLYING_NAME"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$varUnderlyingFUTOption"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varFUTLMEUnderlying">
			<xsl:choose>
				<xsl:when test="$PRANA_UNDERLYING_NAME!=''">
					<xsl:value-of select="$PRANA_UNDERLYING_NAME"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$varUnderlyingFUTLME"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="varFUTUnderlying">
			<xsl:choose>
				<xsl:when test="$PRANA_UNDERLYING_NAME!=''">
					<xsl:value-of select="$PRANA_UNDERLYING_NAME"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="substring(COL20,1,2)"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>

		<xsl:variable name="putCall" select="substring(COL20,5,1)"/>

		<xsl:variable name="ThirdWednesday">
			<xsl:choose>
				<xsl:when test="COL12='FUT' and contains(COL22,'LME')">
					<xsl:value-of select="my:Now1(number(substring(substring-after(COL20,' '),1,4)),number(substring(substring-after(COL20,' '),5,2)))"/>
				</xsl:when>
			</xsl:choose>
		</xsl:variable>

		<xsl:choose>

			<xsl:when test ="$PRANA_SYMBOL_NAME != ''">
				<xsl:value-of select ="$PRANA_SYMBOL_NAME"/>
			</xsl:when>

			<xsl:when test="$PRANA_FLAG!=''">

				<xsl:choose>
					<xsl:when test="COL12='FUT'">

						<xsl:choose>
							<xsl:when test="not(contains(COL22,'LME'))">
								<xsl:value-of select="normalize-space(concat($varFUTUnderlying,' ',$varYear,$varMonthCode,$PRANA_EXCHANGE_NAME))"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test="number(substring-before(substring-after($ThirdWednesday,'/'),'/')) = number($varDate)">
										<xsl:value-of select="concat($varFUTLMEUnderlying,' ',$varYear,$varMonthCode,'-LME')"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="concat($varFUTLMEUnderlying,' ',$varYear,$varMonthCode,$varDate,'-LME')"/>
									</xsl:otherwise>
								</xsl:choose>								
							</xsl:otherwise>
						</xsl:choose>

					</xsl:when>

					<xsl:when test="COL12='FUTOPT'">
						<xsl:value-of select="concat($varFUTOPTUnderlying,' ',$varYear,$varMonthCode,$putCall,$StrikePrice,$PRANA_EXCHANGE_NAME)"/>
					</xsl:when>
				</xsl:choose>

			</xsl:when>

			<xsl:otherwise>
				<xsl:choose>


					<xsl:when test="COL12='FUT'">

						<xsl:choose>

							<xsl:when test="not(contains(COL22,'LME'))">
								<xsl:value-of select="normalize-space(concat($varFUTUnderlying,' ',$varMonthCode,$varYear,$PRANA_EXCHANGE_NAME))"/>
							</xsl:when>

							<xsl:otherwise>
								<xsl:choose>
									<xsl:when test="number(substring-before(substring-after($ThirdWednesday,'/'),'/')) = number($varDate)">
										<xsl:value-of select="concat($varFUTLMEUnderlying,' ',$varYear,$varMonthCode,'-LME')"/>
									</xsl:when>
									<xsl:otherwise>
										<xsl:value-of select="concat($varFUTLMEUnderlying,' ',$varYear,$varMonthCode,$varDate,'-LME')"/>
									</xsl:otherwise>
								</xsl:choose>
								
							</xsl:otherwise>

						</xsl:choose>

					</xsl:when>

					<xsl:when test="COL12='FUTOPT'">
						<xsl:value-of select="concat($varFUTOPTUnderlying,' ',$varMonthCode,$varYear,$putCall,$StrikePrice,$PRANA_EXCHANGE_NAME)"/>
					</xsl:when>
				</xsl:choose>
			</xsl:otherwise>

		</xsl:choose>
	
		
	</xsl:template>
	
	<xsl:template match="/">		
		
		<DocumentElement>

			<xsl:for-each select ="//PositionMaster">
				<xsl:if test ="number(COL66) and COL2='T' and COL12!='CASH'">
					<PositionMaster>
						
						<xsl:variable name="varSymbol">
							<xsl:call-template name="Symbol">
							</xsl:call-template>
						</xsl:variable>

						<Symbol>
							<xsl:value-of select="$varSymbol"/>
						</Symbol>

						<xsl:variable name = "PB_FUND_NAME">
							<xsl:value-of select="COL8"/>
						</xsl:variable>

						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/FundMapping.xml')/FundMapping/PB[@Name='Touradji']/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
						</xsl:variable>

						<AccountName>
							<xsl:choose>

								<xsl:when test ="$PRANA_FUND_NAME!=''">
									<xsl:value-of select ="$PRANA_FUND_NAME"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="$PB_FUND_NAME"/>
								</xsl:otherwise>

							</xsl:choose>
						</AccountName>

						<PositionStartDate>
							<xsl:value-of select ="COL58"/>
						</PositionStartDate>

						<PositionSettlementDate>
							<xsl:value-of select ="COL48"/>
						</PositionSettlementDate>

						<xsl:variable name ="NetPosition">
							<xsl:value-of select ="number(COL66)"/>
						</xsl:variable>
						
						<NetPosition>

							<xsl:choose>

								<xsl:when test ="$NetPosition &lt;0">
									<xsl:value-of select ="$NetPosition*-1"/>
								</xsl:when>

								<xsl:when test ="$NetPosition &gt;0">
									<xsl:value-of select ="$NetPosition"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>

						</NetPosition>

						<xsl:variable name="varside" select="COL60"/>

						<SideTagValue>
							<xsl:choose>
								<xsl:when test="$varside='S'">
									<xsl:value-of select="'2'"/>
								</xsl:when>
								<xsl:when test="$varside='B'">
									<xsl:value-of select="'1'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>

							</xsl:choose>
						</SideTagValue>

						<xsl:variable name ="varCostBasis">
							<xsl:value-of select ="COL69"/>
						</xsl:variable>

						<CostBasis>
							<xsl:choose>

								<xsl:when test ="$varCostBasis &lt;0">
									<xsl:value-of select ="$varCostBasis*-1"/>
								</xsl:when>

								<xsl:when test ="$varCostBasis &gt;0">
									<xsl:value-of select ="$varCostBasis"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</CostBasis>

						<xsl:variable name="varCommision">
							<xsl:value-of select="number(COL78)+ number(COL108)"/>
						</xsl:variable>

						<Commission>
							<xsl:choose>

								<xsl:when test ="$varCommision &lt;0">
									<xsl:value-of select ="$varCommision*-1"/>
								</xsl:when>

								<xsl:when test ="$varCommision &gt;0">
									<xsl:value-of select ="$varCommision"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</Commission>


						<xsl:variable name="MiscFees">
							<xsl:value-of select="number(COL96)"/>
						</xsl:variable>
						
						<MiscFees>
							<xsl:choose>

								<xsl:when test ="$MiscFees &lt;0">
									<xsl:value-of select ="$MiscFees*-1"/>
								</xsl:when>

								<xsl:when test ="$MiscFees &gt;0">
									<xsl:value-of select ="$MiscFees"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</MiscFees>

						
						<xsl:variable name="TaxOnCommissions">
							<xsl:value-of select="number(COL104)"/>
						</xsl:variable>

						<TaxOnCommissions>
							<xsl:choose>

								<xsl:when test ="$TaxOnCommissions &lt;0">
									<xsl:value-of select ="$TaxOnCommissions*-1"/>
								</xsl:when>

								<xsl:when test ="$TaxOnCommissions &gt;0">
									<xsl:value-of select ="$TaxOnCommissions"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</TaxOnCommissions>


						<xsl:variable name="varFee">
							<xsl:value-of select="number(COL92)+number(COL112)"/>
						</xsl:variable>

						<Fees>
							<xsl:choose>

								<xsl:when test ="$varFee &lt;0">
									<xsl:value-of select ="$varFee*-1"/>
								</xsl:when>

								<xsl:when test ="$varFee &gt;0">
									<xsl:value-of select ="$varFee"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</Fees>

						<PBSymbol>
							<xsl:value-of select ="normalize-space(COL29)"/>
						</PBSymbol>


					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>

</xsl:stylesheet>
