<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" xmlns:my="put-your-namespace-uri-here"
>
	<xsl:output method="xml" indent="yes"/>

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

	<xsl:template name="MonthCode">
		<xsl:param name="Month"/>
		<xsl:param name="PutOrCall"/>
		<xsl:if test="$PutOrCall='C'">
			<xsl:choose>
				<xsl:when test="$Month='JAN'">
					<xsl:value-of select="'A'"/>
				</xsl:when>
				<xsl:when test="$Month='FEB' ">
					<xsl:value-of select="'B'"/>
				</xsl:when>
				<xsl:when test="$Month='MAR' ">
					<xsl:value-of select="'C'"/>
				</xsl:when>
				<xsl:when test="$Month='APR' ">
					<xsl:value-of select="'D'"/>
				</xsl:when>
				<xsl:when test="$Month='MAY' ">
					<xsl:value-of select="'E'"/>
				</xsl:when>
				<xsl:when test="$Month='JUN' ">
					<xsl:value-of select="'F'"/>
				</xsl:when>
				<xsl:when test="$Month='JUL'  ">
					<xsl:value-of select="'G'"/>
				</xsl:when>
				<xsl:when test="$Month='AUG'  ">
					<xsl:value-of select="'H'"/>
				</xsl:when>
				<xsl:when test="$Month='SEP' ">
					<xsl:value-of select="'I'"/>
				</xsl:when>
				<xsl:when test="$Month='OCT' ">
					<xsl:value-of select="'J'"/>
				</xsl:when>
				<xsl:when test="$Month='NOV' ">
					<xsl:value-of select="'K'"/>
				</xsl:when>
				<xsl:when test="$Month='DEC' ">
					<xsl:value-of select="'L'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
		<xsl:if test="$PutOrCall='P'">
			<xsl:choose>
				<xsl:when test="$Month='JAN'">
					<xsl:value-of select="'M'"/>
				</xsl:when>
				<xsl:when test="$Month='FEB' ">
					<xsl:value-of select="'N'"/>
				</xsl:when>
				<xsl:when test="$Month='MAR' ">
					<xsl:value-of select="'O'"/>
				</xsl:when>
				<xsl:when test="$Month='APR' ">
					<xsl:value-of select="'P'"/>
				</xsl:when>
				<xsl:when test="$Month='MAY' ">
					<xsl:value-of select="'Q'"/>
				</xsl:when>
				<xsl:when test="$Month='JUN' ">
					<xsl:value-of select="'R'"/>
				</xsl:when>
				<xsl:when test="$Month='JUL'  ">
					<xsl:value-of select="'S'"/>
				</xsl:when>
				<xsl:when test="$Month='AUG'  ">
					<xsl:value-of select="'T'"/>
				</xsl:when>
				<xsl:when test="$Month='SEP' ">
					<xsl:value-of select="'U'"/>
				</xsl:when>
				<xsl:when test="$Month='OCT' ">
					<xsl:value-of select="'V'"/>
				</xsl:when>
				<xsl:when test="$Month='NOV' ">
					<xsl:value-of select="'W'"/>
				</xsl:when>
				<xsl:when test="$Month='DEC' ">
					<xsl:value-of select="'X'"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="''"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:if>
	</xsl:template>

	<xsl:template name="Option">
		<xsl:param name="Symbol"/>
		<xsl:variable name="UnderlyingSymbol">
			<xsl:value-of select="substring-before(normalize-space(COL3),' ')"/>
		</xsl:variable>
		<xsl:variable name="ExpiryYear">
			<xsl:value-of select="substring(substring-before(substring-after(substring-after(substring-after(normalize-space(COL3),' '),' '),' '),' '),3,2)"/>
		</xsl:variable>
		<xsl:variable name="ExpiryMonth">
			<xsl:value-of select="substring-before(substring-after(normalize-space(COL3),' '),' ')"/>
		</xsl:variable>
		<xsl:variable name="ExpiryDay">
			<xsl:value-of select="substring-before(substring-after(substring-after(normalize-space(COL3),' '),' '),' ')"/>
		</xsl:variable>
		<xsl:variable name="PutORCall">
			<xsl:value-of select="substring-after(substring-after(substring-after(substring-after(substring-after(normalize-space(COL3),' '),' '),' '),' '),' ')"/>
		</xsl:variable>
		<xsl:variable name="StrikePrice">
			<xsl:value-of select="format-number(substring-before(substring-after(substring-after(substring-after(substring-after(normalize-space(COL3),' '),' '),' '),' '),' '),'##.00')"/>
		</xsl:variable>

		<xsl:variable name="MonthCodeVar">
			<xsl:call-template name="MonthCode">
				<xsl:with-param name="Month" select="($ExpiryMonth)"/>
				<xsl:with-param name="PutOrCall" select="$PutORCall"/>
			</xsl:call-template>
		</xsl:variable>

		<xsl:variable name="Day">
			<xsl:choose>
				<xsl:when test="substring($ExpiryDay,1,1)='0'">
					<xsl:value-of select="substring($ExpiryDay,2,1)"/>
				</xsl:when>
				<xsl:otherwise>
					<xsl:value-of select="$ExpiryDay"/>
				</xsl:otherwise>
			</xsl:choose>
		</xsl:variable>
		<xsl:value-of select="concat('O:',$UnderlyingSymbol,' ',$ExpiryYear,$MonthCodeVar,$StrikePrice,'D',$Day)"/>
	</xsl:template>


	<xsl:template match="/">

		<DocumentElement>
			<xsl:for-each select ="//Comparision">

				<xsl:variable name="varQuantity">
					<xsl:call-template name="Translate">
						<xsl:with-param name="Number" select="COL5"/>
					</xsl:call-template>
				</xsl:variable>

				<xsl:if test="number($varQuantity)">

					<PositionMaster>

						<xsl:variable name="PB_NAME">
							<xsl:value-of select="'GS'"/>
						</xsl:variable>

						<xsl:variable name="PB_SYMBOL_NAME">
							<xsl:value-of select="normalize-space(COL29)"/>
						</xsl:variable>

						<xsl:variable name="PRANA_SYMBOL_NAME">
							<xsl:value-of select="document('../ReconMappingXML/SymbolMapping.xml')/SymbolMapping/PB[@Name=$PB_NAME]/SymbolData[@PBCompanyName=$PB_SYMBOL_NAME]/@PranaSymbol"/>
						</xsl:variable>

						<xsl:variable name="varAssetType">
							<xsl:choose>
								<xsl:when test="contains(COL19,'P') or contains(COL19,'C')">
									<xsl:value-of select="'EquityOption'"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="'Equity'"/>
								</xsl:otherwise>
							</xsl:choose>
						</xsl:variable>

						<xsl:variable name="varSymbol">
							<xsl:value-of select="normalize-space(COL3)"/>
						</xsl:variable>

						<Symbol>
							<xsl:choose>
								<xsl:when test="$PRANA_SYMBOL_NAME!=''">
									<xsl:value-of select="$PRANA_SYMBOL_NAME"/>
								</xsl:when>
								<xsl:when test="$varAssetType='EquityOption'">
									<xsl:call-template name="Option">
										<xsl:with-param name="Symbol" select="normalize-space(COL3)"/>
									</xsl:call-template>
								</xsl:when>
								<xsl:when test="$varSymbol!=''">
									<xsl:value-of select="$varSymbol"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="$PB_SYMBOL_NAME"/>
								</xsl:otherwise>
							</xsl:choose>
						</Symbol>

						<xsl:variable name="PB_FUND_NAME" select="normalize-space(COL1)"/>

						<xsl:variable name="PRANA_FUND_NAME">
							<xsl:value-of select ="document('../ReconMappingXML/AccountMapping.xml')/FundMapping/PB[@Name=$PB_NAME]/FundData[@PBFundName=$PB_FUND_NAME]/@PranaFund"/>
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

						<Quantity>
							<xsl:choose>
								<xsl:when test="number($varQuantity)">
									<xsl:value-of select="$varQuantity"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</Quantity>

						<xsl:variable name="varSide">
							<xsl:value-of select="COL4"/>
						</xsl:variable>

						<Side>
							<xsl:choose>
								<xsl:when test="$varAssetType='EquityOption'">
									<xsl:choose>
										<xsl:when test="$varSide='L'">
											<xsl:value-of select="'Buy to Open'"/>
										</xsl:when>
										<xsl:when test="$varSide='S'">
											<xsl:value-of select="'Sell to Close'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:when>
								<xsl:otherwise>
									<xsl:choose>
										<xsl:when test="$varSide='L'">
											<xsl:value-of select="'Buy'"/>
										</xsl:when>
										<xsl:when test="$varSide='S'">
											<xsl:value-of select="'Sell short'"/>
										</xsl:when>
										<xsl:otherwise>
											<xsl:value-of select="''"/>
										</xsl:otherwise>
									</xsl:choose>
								</xsl:otherwise>
							</xsl:choose>
						</Side>

						<xsl:variable name="MarkPrice">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL6"/>
							</xsl:call-template>
						</xsl:variable>

						<MarkPrice>
							<xsl:choose>
								<xsl:when test="number($MarkPrice)">
									<xsl:value-of select="format-number($MarkPrice,'0.0000')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarkPrice>

						<xsl:variable name="MarketValue">
							<xsl:call-template name="Translate">
								<xsl:with-param name="Number" select="COL7"/>
							</xsl:call-template>
						</xsl:variable>

						<MarketValue>
							<xsl:choose>
								<xsl:when test="number($MarketValue)">
									<xsl:value-of select="format-number($MarketValue,'0.0000')"/>
								</xsl:when>
								<xsl:otherwise>
									<xsl:value-of select="0"/>
								</xsl:otherwise>
							</xsl:choose>
						</MarketValue>

					</PositionMaster>
				</xsl:if>
			</xsl:for-each>
		</DocumentElement>
	</xsl:template>

</xsl:stylesheet>
