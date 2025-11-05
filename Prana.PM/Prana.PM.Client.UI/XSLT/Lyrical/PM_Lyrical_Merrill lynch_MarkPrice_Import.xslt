<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xs="http://www.w3.org/2001/XMLSchema" exclude-result-prefixes="xs"
xmlns:msxsl="urn:schemas-microsoft-com:xslt"
    xmlns:my="put-your-namespace-uri-here">
	<xsl:output method="xml" encoding="UTF-8" indent="yes"/>

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
	</xsl:template>

	<xsl:template match="/">

		<DocumentElement>

			<xsl:for-each select ="//PositionMaster">

				<!--<xsl:variable name="PositionPrefix">
					<xsl:value-of select="substring(substring-before(substring-after(substring-after(COL1,'+'),'A'),'+'),1,string-length(substring-before(substring-after(substring-after(COL1,'+'),'A'),'+'))-6)"/>
				</xsl:variable>

				<xsl:variable name="PositionSuffix">
					<xsl:value-of select="substring(substring-before(substring-after(substring-after(COL1,'+'),'A'),'+'),string-length(substring-before(substring-after(substring-after(COL1,'+'),'A'),'+'))-5)"/>
				</xsl:variable>

				<xsl:variable name="Position">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="concat($PositionPrefix,'.',$PositionSuffix)"/>
					</xsl:call-template>
				</xsl:variable>-->

				<xsl:variable name="CostCol">
					<xsl:value-of select="substring-before(substring-after(substring-after(normalize-space(COL1),' '),' '),'+')"/>
				</xsl:variable>

				<xsl:variable name="CostPre">
					<xsl:value-of select="substring($CostCol,1,string-length($CostCol)-3)"/>
				</xsl:variable>

				<xsl:variable name="CostSuff">
					<xsl:value-of select="substring($CostCol,string-length($CostCol)-2)"/>
				</xsl:variable>

				<xsl:variable name="Cost">
					<xsl:value-of select="number(concat($CostPre,'.',$CostSuff))"/>
				</xsl:variable>

				<xsl:variable name="SymbolChk">
					<xsl:value-of select="substring-before(substring-after(normalize-space(COL1),' '),' ')"/>
				</xsl:variable>

				<xsl:if test="number($Cost) and (not(contains($SymbolChk,'SD1')) and not(contains($SymbolChk,'SD2')) and not(contains($SymbolChk,'SD3')))">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'Merrill Lynch'"/>
						</xsl:variable>

						<xsl:variable name = "PB_SYMBOL_NAME" >
							<xsl:value-of select ="''"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXml/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="Symbol">
							<xsl:choose>
								<xsl:when test="string-length($SymbolChk)=20">
									<xsl:value-of select="substring($SymbolChk,string-length($SymbolChk)-4)"/>
								</xsl:when>
								<xsl:when test="string-length($SymbolChk)=19">
									<xsl:value-of select="substring($SymbolChk,string-length($SymbolChk)-3)"/>
								</xsl:when>
								<xsl:when test="string-length($SymbolChk)=18">
									<xsl:value-of select="substring($SymbolChk,string-length($SymbolChk)-2)"/>
								</xsl:when>
								<xsl:when test="string-length($SymbolChk)=17">
									<xsl:value-of select="substring($SymbolChk,string-length($SymbolChk)-1)"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="''"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<Symbol>
							<xsl:choose>

								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>

								<xsl:when test="$Symbol!='*'">
									<xsl:value-of select="normalize-space($Symbol)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>

							</xsl:choose>
						</Symbol>

						<!--<xsl:variable name="PB_FUND_NAME" select="substring-before(COL1,' ')"/>

						<xsl:variable name ="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXml/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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
						</AccountName>-->

						<!--<xsl:variable name="Side" select="normalize-space(COL13)"/>-->

						<!--<xsl:variable name="PositionSign" select="substring(substring-after(COL1,'+'),17,1)"/>

						<SideTagValue>
							<xsl:choose>
								<xsl:when test="$PositionSign='+'">
									<xsl:value-of select="'1'"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="'2'"/>
								</xsl:otherwise>

							</xsl:choose>
						</SideTagValue>

						<NetPosition>
							<xsl:choose>

								<xsl:when test="$Position &gt; 0">
									<xsl:value-of select="$Position"/>
								</xsl:when>

								<xsl:when test="$Position &lt; 0">
									<xsl:value-of select="$Position * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</NetPosition>-->

						


						<MarkPrice>
							<xsl:choose>

								<xsl:when test="$Cost &gt; 0">
									<xsl:value-of select="$Cost"/>
								</xsl:when>

								<xsl:when test="$Cost &lt; 0">
									<xsl:value-of select="$Cost * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
						</MarkPrice>

						<xsl:variable name="TradeDate" select="substring(COL1,492,8)"/>

						<Date>
							<!--<xsl:value-of select="substring(COL1,492,8)"/>-->
							<!--<xsl:value-of select="concat(substring($TradeDate,5,2),'/',substring($TradeDate,7,2),'/',substring($TradeDate,1,4))"/>-->
						</Date>

						<PBSymbol>
							<xsl:value-of select ="normalize-space($PB_SYMBOL_NAME)"/>
						</PBSymbol>

						<!--<xsl:variable name="Commission" select="number(COL28)"/>-->

						<!--<xsl:variable name="Commission">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL28"/>
							</xsl:call-template>
						</xsl:variable>

						<Commission>
							-->
						<!--<xsl:choose>
								<xsl:when test="COL12='R'">
									<xsl:choose>
										<xsl:when test ="number($Commission)">
											<xsl:value-of select="$Commission*-1"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select ="0"/>
										</xsl:otherwise>
									</xsl:choose>

								</xsl:when>
								<xsl:otherwise>-->
						<!--
							<xsl:choose>

								<xsl:when test="$Commission &gt; 0">
									<xsl:value-of select="$Commission"/>
								</xsl:when>

								<xsl:when test="$Commission &lt; 0">
									<xsl:value-of select="$Commission * (-1)"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>

							</xsl:choose>
							-->
						<!--</xsl:otherwise>
							</xsl:choose>-->
						<!--


						</Commission>-->

						<!--<xsl:variable name ="varPurchaseDate">
							<xsl:choose>

								<xsl:when test="COL5='*'">
									<xsl:value-of select="''"/>
								</xsl:when>

								<xsl:otherwise>
									<xsl:value-of select ="COL5"/>
								</xsl:otherwise>

							</xsl:choose>
						</xsl:variable>

						<OriginalPurchaseDate>
							<xsl:value-of select="$varPurchaseDate"/>
						</OriginalPurchaseDate>-->

					</PositionMaster>

				</xsl:if>

			</xsl:for-each>

		</DocumentElement>

	</xsl:template>
	<xsl:variable name="varCapital" select="'ABCDEFGHIJKLMNOPQRSTUVWXYZ'"/>
	<xsl:variable name="varSmall" select="'abcdefghijklmnopqrstuvwxyz'"/>
</xsl:stylesheet>
